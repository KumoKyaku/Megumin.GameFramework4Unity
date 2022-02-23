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
        猥亵值,
    }

    [Serializable]
    public class NumericalProperty
    {
        public NumericalType Type;
        public int Current;
        public int Max;

        public NumericalProperty Clone()
        {
            return this.MemberwiseClone() as NumericalProperty;
        }
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
        public void CalFinalChangeValue(Dictionary<NumericalType, NumericalProperty> currentProp)
        {
            float result = ConstValue;

            if (currentProp != null)
            {
                if (currentProp.TryGetValue(Type, out var myProp))
                {
                    var mop = MaxValueFactor * myProp.Max;
                    result += mop;
                }

                foreach (var item in OtherFactor)
                {
                    if (currentProp.TryGetValue(item.Type, out var prop))
                    {
                        var op = item.Factor * prop.Current;
                        result += op;
                    }
                }
            }

            result *= GlobalFactor;

            FinalChangeValue = (int)result;
        }
    }

    public class NumericalObject
    {
        public Dictionary<NumericalType, NumericalProperty> NumericalProperty { get; internal set; }
            = new Dictionary<NumericalType, NumericalProperty>();

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
                    todo.PropertyChange.CalFinalChangeValue(NumericalProperty);
                    var newv = prop.Current + todo.PropertyChange.FinalChangeValue;
                    newv = Math.Min(newv, prop.Max);
                    prop.Current = newv;
                    todo.ResultSource?.SetResult(0);
                }
                else
                {
                    todo.ResultSource?.SetResult(-1);
                }
            }
        }
    }


}
