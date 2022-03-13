using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Megumin;

namespace Megumin.GameFramework.Numerical
{
    public enum NumericalType
    {
        HP,
        MP,
        SP,
        攻击力,
        防御力,
        智力,
        力量,
        敏捷,
        精神,

        冲击力,
        黄色护盾,
        蓝色护盾,
        仇恨,
        躯干,
        威胁值,

        HPMax,
        MPMax,
        SPMax,
        躯干Max,
    }

    [Obsolete("错误设计，HP，和 HPMax 是两个属性，不能写成一个", true)]
    [Serializable]
    public class NumericalProperty
    {
        public SEnum<NumericalType> Type;
        public int Current;
        public int Max;

        public NumericalProperty Clone()
        {
            return this.MemberwiseClone() as NumericalProperty;
        }
    }

    public sealed class ExtraValueIntMultipleValue : IntMultipleValue<object>
    {
        //public ExtraValueIntMultipleValue(object key, int v = default)
        //    :base(key, v)
        //{

        //}

        public override int RemoveAll()
        {
            ElementDic.Clear();
            //ElementDic[DefaultKey] = DefaultValue;
            ApplyValue();
            return Current;
        }
    }

    /// <summary>
    /// 不要序列化这个类
    /// </summary>
    public class NumericalProperty2
    {
        public NumericalType Type;

        /// <summary>
        /// 基础数值/白字部分
        /// </summary>
        public int BaseValue;
        public int ExtraValue { get; protected set; }

        /// <summary>
        /// buff装备增减值
        /// </summary>
        public IntMultipleValue<object> MultipleExtraValue;

        public int Value => BaseValue + ExtraValue;

        /// <summary>
        /// 对应关系直接写死。
        /// </summary>
        /// <returns></returns>
        public NumericalType? GetMaxType()
        {
            switch (Type)
            {
                case NumericalType.HP:
                    return NumericalType.HPMax;
                case NumericalType.MP:
                    return NumericalType.MPMax;
                case NumericalType.SP:
                    return NumericalType.SPMax;
                case NumericalType.躯干:
                    return NumericalType.躯干Max;
                default:
                    break;
            }
            return null;
        }

        void ExtraChanged(int newValue, int oldValue)
        {
            ExtraValue = newValue;
        }

        public NumericalProperty2()
        {
            MultipleExtraValue = new IntMultipleValue<object>();
            MultipleExtraValue.ValueChanged += ExtraChanged;
        }
    }

    public interface INumericalPropertyFinder
    {
        //int? Find(NumericalType type);
        bool TryGetValue(NumericalType type, out NumericalProperty2 value);
    }

    [Serializable]
    public class PropertyChange
    {
        public NumericalType Type;
        /// <summary>
        /// 常数项
        /// </summary>
        public int ConstValue = 0;
        /// <summary>
        /// 最大值系数
        /// </summary>
        public float MaxValueFactor = 0;

        /// <summary>
        /// 受其他属性影响的系数,比如额外恢复防御力5%的血量
        /// </summary>
        [Serializable]
        public class PropertyFactor
        {
            public NumericalType Type;
            public float Factor = 0f;
            [HelpBox("仅对白字部分生效还是整个值生效。")]
            //其他属性用白字值，避免循环计算。
            public bool IsBase = true;
        }

        [SerializeField]
        private List<PropertyFactor> OtherFactor = new List<PropertyFactor>();

        [Space]
        [ProtectedInInspector]
        public float GlobalFactor = 1;

        public PropertyChange Clone()
        {
            return this.MemberwiseClone() as PropertyChange;
        }

        public int FinalChangeValue { get; protected set; }

        /// <summary>
        /// 计算各个因子影响后的结果,如果以后有需要，可以创建公式SO文件来定义不同公式。
        /// </summary>
        /// <param name="currentProp"></param>
        /// <remarks>各个因子只能对白字生效，否则会循环计算导致数据爆炸</remarks>
        public void CalFinalChangeValue(INumericalPropertyFinder finder)
        {
            float result = ConstValue;

            if (finder != null)
            {
                if (finder.TryGetValue(Type, out var myProp))
                {
                    var maxType = myProp.GetMaxType();
                    if (maxType.HasValue)
                    {
                        if (finder.TryGetValue(maxType.Value, out var maxProp))
                        {
                            //最大值用实际值
                            var mop = MaxValueFactor * maxProp.Value;
                            result += mop;
                        }
                    }
                }
                
                foreach (var item in OtherFactor)
                {
                    if (finder.TryGetValue(item.Type, out var prop))
                    {
                        var pv = prop.BaseValue;
                        if (!item.IsBase)
                        {
                            pv = prop.Value;
                        }

                        var op = item.Factor * pv;
                        result += op;
                    }
                }
            }

            result *= GlobalFactor;

            FinalChangeValue = (int)result;
        }
    }

    public class NumericalObject : INumericalPropertyFinder
    {
        public Dictionary<NumericalType, NumericalProperty2> NumericalProperty { get; internal set; }
            = new Dictionary<NumericalType, NumericalProperty2>();

        public Task<int> ChangePropertyOnFrameline(PropertyChange change) => ChangePropertyAsync(change);

        Task<int> ChangePropertyAsync(PropertyChange propertyChange)
        {
            TaskCompletionSource<int> source = new TaskCompletionSource<int>();
            TodoChange change = new TodoChange();
            change.Type = propertyChange.Type;
            change.PropertyChange = propertyChange;
            change.ResultSource = source;
            Todos.Enqueue(change);
            return source.Task;
        }

        internal class TodoChange
        {
            public TaskCompletionSource<int> ResultSource { get; internal set; }
            public NumericalType Type { get; internal set; }
            public PropertyChange PropertyChange { get; internal set; }
        }

        Queue<TodoChange> Todos = new Queue<TodoChange>();
        public void ApplyChangeList()
        {
            while (Todos.TryDequeue(out var todo))
            {
                if (NumericalProperty.TryGetValue(todo.Type, out var prop))
                {
                    todo.PropertyChange.CalFinalChangeValue(this);
                    var newv = prop.BaseValue + todo.PropertyChange.FinalChangeValue;
                    var maxType = prop.GetMaxType();
                    if (maxType.HasValue)
                    {
                        if (NumericalProperty.TryGetValue(maxType.Value, out var maxProp))
                        {
                            //限制最大值。
                            newv = Math.Min(newv, maxProp.Value);
                        }
                    }

                    prop.BaseValue = newv;
                    todo.ResultSource?.SetResult(0);
                }
                else
                {
                    todo.ResultSource?.SetResult(-1);
                }
            }
        }

        public bool TryGetValue(NumericalType type, out NumericalProperty2 value)
        {
            return NumericalProperty.TryGetValue(type, out value);
        }
    }


}
