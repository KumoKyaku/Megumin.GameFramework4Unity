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
            var old = Value;
            ExtraValue = newValue;
            ValueChange?.Invoke(Value, old);
        }

        public NumericalProperty2()
        {
            MultipleExtraValue = new IntMultipleValue<object>();
            MultipleExtraValue.ValueChanged += ExtraChanged;
        }

        public event Megumin.OnValueChanged<int> ValueChange;
    }

    public interface INumericalPropertyFinder
    {
        //int? Find(NumericalType type);
        bool TryGetValue(NumericalType type, out NumericalProperty2 value);
    }

    [Serializable]
    public class PropertyChange : ISerializationCallbackReceiver
    {
        [HideInInspector]
        public string ElementName;

        [UnityEngine.Serialization.FormerlySerializedAs("Type")]
        public NumericalType TargetType;
        /// <summary>
        /// 常数项
        /// </summary>
        public int ConstValue = 0;
        
        [HelpBox("在所有因子计算完毕后应用这个因子,默认值为 1")]
        [ProtectedInInspector]
        public float GlobalFactor = 1;

        /// <summary>
        /// 受其他属性影响的系数,比如额外恢复防御力5%的血量
        /// </summary>
        [Serializable]
        public class PropertyFactor:ISerializationCallbackReceiver
        {
            [HideInInspector]
            public string ElementName;

            [UnityEngine.Serialization.FormerlySerializedAs("Type")]
            public NumericalType FactorType;
            public float BaseFactor = 0f;
            public float ExtraFactor = 0f;
            public float Factor = 0f;

            public void OnBeforeSerialize()
            {
                ElementName = FactorType.ToString();
            }

            public void OnAfterDeserialize()
            {
                
            }
        }

        [Space]
        [SerializeField]
        private List<PropertyFactor> Factor = new List<PropertyFactor>();

        public PropertyChange Clone()
        {
            return this.MemberwiseClone() as PropertyChange;
        }

        public int LastCalValue { get; protected set; }

        /// <summary>
        /// 计算各个因子影响后的结果,如果以后有需要，可以创建公式SO文件来定义不同公式。
        /// </summary>
        /// <param name="currentProp"></param>
        /// <remarks>各个因子只能对白字生效，否则会循环计算导致数据爆炸</remarks>
        public int CalFinalChangeValue(INumericalPropertyFinder finder)
        {
            float result = ConstValue;

            if (finder != null)
            {
                foreach (var item in Factor)
                {
                    if (finder.TryGetValue(item.FactorType, out var prop))
                    {
                        result += prop.BaseValue * item.BaseFactor;
                        result += prop.ExtraValue * item.ExtraFactor;
                        result += prop.Value * item.Factor;
                    }
                }
            }

            result *= GlobalFactor;

            LastCalValue = (int)result;
            return LastCalValue;
        }

        public void OnBeforeSerialize()
        {
            ElementName = TargetType.ToString();
        }

        public void OnAfterDeserialize()
        {
            
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
            change.Type = propertyChange.TargetType;
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
                Debug.Log("ApplyChangeList");
                if (NumericalProperty.TryGetValue(todo.Type, out var prop))
                {
                    todo.PropertyChange.CalFinalChangeValue(this);
                    var newv = prop.BaseValue + todo.PropertyChange.LastCalValue;
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

        public int ChangeProperty(PropertyChange PropertyChange)
        {
            if (NumericalProperty.TryGetValue(PropertyChange.TargetType,out var prop))
            {
                PropertyChange.CalFinalChangeValue(this);
                var newv = prop.BaseValue + PropertyChange.LastCalValue;
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
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public bool TryGetValue(NumericalType type, out NumericalProperty2 value)
        {
            return NumericalProperty.TryGetValue(type, out value);
        }

        public NumericalProperty2 GetAutoAdd(NumericalType type)
        {
            if (NumericalProperty.TryGetValue(type, out var value))
            {
                return value;
            }
            else
            {
                value = new NumericalProperty2();
                value.Type = type;
                NumericalProperty.Add(type, value);
            }
            return value;
        }
    }


}
