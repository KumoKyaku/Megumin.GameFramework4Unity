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
        public double DValue { get; private set; }
        public bool BroadCast = false;

        public List<ChildPorperty> RefBy = new List<ChildPorperty>();

        public virtual void ReCalRefBy()
        {
            if (BroadCast)
            {
                foreach (var item in RefBy)
                {
                    item.ReCalRefBy();
                }
            }
        }

        protected void SetNewValue(double newValue)
        {
            if (DValue != newValue)
            {
                DValue = newValue;
                ReCalRefBy();
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
            public double factor;
            public object source;

            public double Cal()
            {
                double v = ConstValue + Connect?.DValue ?? 0;
                v *= factor;
                return v;
            }
        }

        protected List<ChildProperty> chileds { get; } = new List<ChildProperty>();

        public void Add(object souce, double baseV, ChildPorperty property, double factor)
        {
            ChildProperty child = new ChildProperty()
            {
                ConstValue = baseV,
                Connect = property,
                source = souce,
                factor = factor,
            };

            if (property != null)
            {
                property.RefBy.Add(this);
            }

            CalV();
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
            chileds.RemoveAll(ele => ele.source == source);
            CalV();
        }

        public void CalV()
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

    public class SumChildPopperty : CombinePoperty
    {
        protected override double OprationChild()
        {
            double v = 0;
            foreach (var item in chileds)
            {
                v += item.Cal();
            }

            return v;
        }
    }

    public class LayerProperty : CombinePoperty
    {
        public SumChildPopperty LayerScale;

        public void SetLayerScale(SumChildPopperty property)
        {
            LayerScale = property;
            property.RefBy.Add(this);

            CalV();
        }

        protected override double OprationChild()
        {
            double sum = 0;
            foreach (var item in chileds)
            {
                sum += item.Cal();
            }
            var v = sum * (1 + LayerScale.DValue);
            return v;
        }
    }
}
