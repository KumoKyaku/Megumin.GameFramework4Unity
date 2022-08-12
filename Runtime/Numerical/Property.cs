using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Numerical
{
    /// <summary>
    /// 可作为别的属性的子的属性
    /// </summary>
    public class ChildPorperty
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
        public List<ChildPorperty> RefBy = new List<ChildPorperty>();

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
    public class ConstValuePorperty : ChildPorperty
    {
        public void SetValue(double Value)
        {
            SetNewValue(Value);
        }
    }

    /// <summary>
    /// 组合属性，需要运算的属性，多个子运算得到结果
    /// </summary>
    public abstract class CombinePoperty : ChildPorperty
    {
        public class ChildProperty
        {
            public double ConstValue;
            public ChildPorperty Connect;
            public double Scale;
            public object Source;

            public double Cal()
            {
                double v = ConstValue + Connect?.DValue ?? 0;
                v *= Scale;
                return v;
            }

            public override string ToString()
            {
                return $"{Source}--{Connect}--ConstValue:{ConstValue}--Scale:{Scale}";
            }
        }

        protected List<ChildProperty> children { get; } = new List<ChildProperty>();

        public void Add(object souce, double baseV, ChildPorperty property, double factor)
        {
            ChildProperty child = new ChildProperty()
            {
                ConstValue = baseV,
                Connect = property,
                Source = souce,
                Scale = factor,
            };

            children.Add(child);

            if (property != null)
            {
                property.RefBy.Add(this);
            }

            ReCalValue();
        }

        public void Add(ChildPorperty p)
        {
            Add(null, 0, p, 1);
        }

        public void Add(object souce, ChildPorperty p, double factor = 1)
        {
            Add(souce, 0, p, factor);
        }

        public void Add(object souce, double constV, double factor = 1)
        {
            Add(souce, constV, null, factor);
        }

        public void Remove(object source)
        {
            children.RemoveAll(ele => ele.Source == source);
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
            foreach (var item in children)
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
            foreach (var item in children)
            {
                sum += item.Cal();
            }
            var v = sum * (1 + LayerScale?.DValue ?? 0);
            return v;
        }
    }

    public class ItemPropertyPostCombindProp
    {
        public ConstValuePorperty 基础值 = new();

        public SumChildPopperty 装备固定加成 = new();
        public SumChildPopperty 装备系数加成 = new();
        public LayerProperty 力装备加成后总计 = new();

        public SumChildPopperty 属性关联固定加成 = new();
        public SumChildPopperty 属性关联系数加成 = new();
        public LayerProperty 属性关联后总计 = new();

        public SumChildPopperty 后期固定加成 = new();
        public SumChildPopperty 后期系数加成 = new();
        public LayerProperty 面板值 = new();
    }
}
