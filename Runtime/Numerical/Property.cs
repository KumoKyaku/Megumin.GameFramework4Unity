using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Numerical
{
    /// <summary>
    /// 可作为别的属性的子的属性
    /// </summary>
    public abstract class Porperty
    {
        public string Type;
        public string MainType;
        public string SubType;
        public int Value { get; private set; }
        public double DValue { get; private set; }
        public bool BroadCast = false;

        /// <summary>
        /// Todo改为事件，事件节省空间，不用申请数组内存
        /// </summary>
        public List<Porperty> RefBy = new List<Porperty>();

        public virtual void BroadCastRefBy()
        {
            if (BroadCast)
            {
                foreach (var item in RefBy)
                {
                    item.ReCalValue();
                }
            }
        }

        public virtual void ReCalValue() { }

        protected void SetNewValue(double newValue)
        {
            if (DValue != newValue)
            {
                DValue = newValue;
                Value = (int)newValue;
                BroadCastRefBy();
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Type))
            {
                return base.ToString();
            }
            else
            {
                return Type;
            }
        }
    }

    /// <summary>
    /// 常数属性
    /// </summary>
    public class ConstValuePorperty : Porperty
    {
        public void SetValue(double Value)
        {
            SetNewValue(Value);
        }
    }

    public class ChildProperty
    {
        public double ConstValue;
        public Porperty Connect;
        public double Scale = 1;
        public object Source;

        /// <summary>
        /// TODO 多态此类型，优化计算？优化计算和虚函数哪个效率更好？
        /// </summary>
        /// <returns></returns>
        public double Cal()
        {
            double v = ConstValue;
            if (Connect != null)
            {
                v += Connect.DValue;
            }
            v *= Scale;
            return v;
        }

        public override string ToString()
        {
            return $"{Source}--{Connect}--ConstValue:{ConstValue}--Scale:{Scale}";
        }
    }

    /// <summary>
    /// 组合属性，需要运算的属性，多个子运算得到结果
    /// </summary>
    public abstract class CombinePoperty : Porperty
    {
        protected List<ChildProperty> Children { get; } = new List<ChildProperty>();

        public void Add(object souce, double baseV, Porperty property, double scale)
        {
            ChildProperty child = new ChildProperty()
            {
                ConstValue = baseV,
                Connect = property,
                Source = souce,
                Scale = scale,
            };

            Add(child);
        }

        public void Add(ChildProperty child)
        {
            Children.Add(child);

            if (child.Connect != null)
            {
                child.Connect.RefBy.Add(this);
            }

            ReCalValue();
        }

        public void Add(Porperty p)
        {
            Add(null, 0, p, 1);
        }

        public void Add(object souce, Porperty p, double scale = 1)
        {
            Add(souce, 0, p, scale);
        }

        public void Add(object souce, double constV, double scale = 1)
        {
            Add(souce, constV, null, scale);
        }

        public void Remove(object source)
        {
            Children.RemoveAll(ele => ele.Source == source);
            ReCalValue();
        }

        public override void ReCalValue()
        {
            double v = OprationChild();

            SetNewValue(v);
        }

        /// <summary>
        /// 对子项进行运算
        /// </summary>
        /// <returns></returns>
        protected abstract double OprationChild();
    }

    public sealed class SumChildPopperty : CombinePoperty
    {
        protected override double OprationChild()
        {
            double v = 0;
            foreach (var item in Children)
            {
                v += item.Cal();
            }

            return v;
        }
    }

    public sealed class LayerProperty : CombinePoperty
    {
        public SumChildPopperty LayerScale;

        public void SetLayerScale(SumChildPopperty property)
        {
            LayerScale = property;
            property.RefBy.Add(this);

            ReCalValue();
        }

        protected override double OprationChild()
        {
            double sum = 0;
            foreach (var item in Children)
            {
                sum += item.Cal();
            }
            var v = sum * (1 + LayerScale?.DValue ?? 0);
            return v;
        }
    }


    /// <summary>
    /// 固定数值
    /// </summary>
    [Obsolete("不良设计", true)]
    class FixValueActor
    {
        int HPMax;
        int HP;
        int MPMax;
        int MP;

        public int GetProp(string type)
        {
            switch (type)
            {
                default:
                    break;
            }
            return default;
        }
    }

    /// <summary>
    /// 双向链表结构
    /// </summary>
    [Obsolete("不良设计", true)]
    class LinkNodeProp
    {
        LinkNodeProp Prev;
        LinkNodeProp Next;
        int Offset;
        double scalle;
        double CurrentValue;
    }
}
