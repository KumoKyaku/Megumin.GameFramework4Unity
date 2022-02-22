using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin.GameFramework.Numerical
{
    public enum NumericalType
    {
        HP,
        冲击力,
        黄色护盾,
        蓝色护盾,
        仇恨,
        躯干,
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

    public class PropertyChange
    {
        public NumericalType Type { get; set; }
        public int ChangeValue { get; set; }
    }

    public class NumericalObject
    {
        public Dictionary<NumericalType, NumericalProperty> NumericalProperty { get; internal set; }
            = new Dictionary<NumericalType, NumericalProperty>();

        public Task<int> ChangePropertyOnFrameline(NumericalType type, int changeValue) => ChangePropertyAsync(type, changeValue);

        Task<int> ChangePropertyAsync(NumericalType type,int changeValue)
        {
            TaskCompletionSource<int> source = new TaskCompletionSource<int>();
            TodoChange change = new TodoChange();
            change.Type = type;
            change.ChangeValue = changeValue;
            change.ResultSource = source;
            Todos.Enqueue(change);
            return source.Task;
        }

        internal class TodoChange
        {
            public TaskCompletionSource<int> ResultSource { get; internal set; }
            public NumericalType Type { get; internal set; }
            public int ChangeValue { get; internal set; }
        }

        Queue<TodoChange> Todos = new Queue<TodoChange>();
        public void ApplyChangeList()
        {
            while (Todos.TryDequeue(out var change))
            {
                if (NumericalProperty.TryGetValue(change.Type, out var prop))
                {
                    var newv = prop.Current + change.ChangeValue;
                    newv = Math.Min(newv, prop.Max);
                    prop.Current = newv;
                    change.ResultSource?.SetResult(0);
                }
                else
                {
                    change.ResultSource?.SetResult(-1);
                }
            }
        }
    }

    
}
