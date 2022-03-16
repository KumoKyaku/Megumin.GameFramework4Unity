using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin.GameFramework.Numerical
{
    public class TempProp : IConsumable
    {
        public int ConsumableCount { get; set; }
        public bool Use(int count, out int used)
        {
            if (ConsumableCount >= count)
            {
                ConsumableCount -= count;
                used = count;
                return true;
            }
            else
            {
                ConsumableCount = 0;
                used = ConsumableCount;
                return false;
            }
        }
    }

    public class TimelimitedProperty<T>
    {
        protected List<IConsumable> List { get; } = new List<IConsumable>();
        public T Type;
        public int Current;

        public void Cal()
        {
            Current = 0;
            foreach (var item in List)
            {
                Current += item.ConsumableCount;
            }
        }

        public virtual void Add(IConsumable propInstance)
        {
            List.Add(propInstance);
            //Cal();
            Current += propInstance.ConsumableCount;
            Debug.Log($"获得临时数值 {Type} 增加{propInstance.ConsumableCount} ，现有 {Current}");
        }

        public virtual void Remove(IConsumable propInstance)
        {
            List.Remove(propInstance);
            //Cal();
            Current -= propInstance.ConsumableCount;
            Debug.Log($"失去临时数值 {Type}  减少{propInstance.ConsumableCount} ，现有 {Current}");
        }

        //public virtual Span<IConsumable> GetSortedUseTarget()
        //{
        //    return List.;
        //}

        public bool TryUse(int count)
        {
            if (Current >= count)
            {
                //TODO,排序。
                foreach (var item in List)
                {
                    if (item.Use(count, out int used))
                    {
                        break;
                    }
                    else
                    {
                        count -= used;
                    }
                }

                List.RemoveAll((item) =>
                {
                    if (item.ConsumableCount <= 0)
                    {
                        return true;
                    }
                    return false;
                });

                Cal();
                Debug.Log($"使用临时数值 {Type}  减少{count} ，现有 {Current}");
                return true;
            }
            return false;
        }
    }

    public abstract class TimelimitedPropertyObject<P, T>
        where P : TimelimitedProperty<T>, new()
    {
        public Dictionary<T, P> lifePorpDic { get; } = new Dictionary<T, P>();

        public bool TryGet(T type, out P p) => lifePorpDic.TryGetValue(type, out p);

        public P GetAutoAdd(T type)
        {
            if (lifePorpDic.TryGetValue(type, out var p))
            {

            }
            else
            {
                p = new();
                p.Type = type;
                lifePorpDic.Add(type, p);
            }

            return p;
        }

        /// <summary>
        /// 特殊属性，先在这里做，然后抽象到数值系统。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="duration"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void PushTemp(T type, int count, float duration)
        {
            var p = GetAutoAdd(type);
            Push(count, duration, p);
        }

        public abstract ITimer Timer { get; }

        protected async void Push(int count, float duration, P p)
        {
            TempProp propInstance = new TempProp();
            propInstance.ConsumableCount = count;
            p.Add(propInstance);
            await Timer.Wait(duration);
            p.Remove(propInstance);
        }
    }



}


