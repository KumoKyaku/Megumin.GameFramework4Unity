using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace Megumin.GameFramework.Numerical
{
    /// <summary>
    /// 数值属性的最基本功能
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// 当前值
        /// </summary>
        int Value { get; }
        event OnValueChanged<int> ValueChange;
    }

    public interface IPropertyWithSource : IProperty
    {
        /// <summary>
        /// 最后数值变动的原因
        /// </summary>
        object Source { get; }
        event OnValueChanged<(object Source, int Value)> SourceValueChanged;
    }

    /// <summary>
    /// 可作为别的属性的子的属性
    /// </summary>
    public abstract class Porperty : IProperty, IPropertyWithSource
    {
        public string Type;
        public string MainType;
        public string SubType;
        public int Value { get; private set; }
        /// <summary>
        /// 通常用于中间环节计算，防止累计误差
        /// </summary>
        public float FValue { get; private set; }
        public bool BroadCast = false;
        /// <summary>
        /// 最后数值变动的原因
        /// </summary>
        public object Source { get; private set; }

        /// <summary>
        /// Todo改为事件，事件节省空间，不用申请数组内存
        /// </summary>
        public List<Porperty> RefBy = new List<Porperty>();

        public virtual void BroadCastRefBy(object source)
        {
            if (BroadCast)
            {
                foreach (var item in RefBy)
                {
                    item.ReCalValue(source);
                }
            }
        }

        public virtual void ReCalValue(object source) { }

        public event OnValueChanged<int> ValueChange;
        public event OnValueChanged<(object Source, int Value)> SourceValueChanged;

        protected void SetNewValue(object source, float newValue)
        {
            if (FValue != newValue)
            {
                var old = Value;
                var oldSource = Source;
                FValue = newValue;
                Value = (int)newValue;
                Source = source;
                ValueChange?.Invoke(Value, old);
                SourceValueChanged?.Invoke((source, Value), (oldSource, old));
                BroadCastRefBy(source);
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
        public void SetValue(float Value)
        {
            SetNewValue(null, Value);
        }

        public void SetValue(object source, float Value)
        {
            SetNewValue(source, Value);
        }
    }

    public class ChildProperty
    {
        public float ConstValue;
        public Porperty Connect;
        public float Scale = 1;
        public object Source;

        /// <summary>
        /// TODO 多态此类型，优化计算？优化计算和虚函数哪个效率更好？
        /// </summary>
        /// <returns></returns>
        public float Cal()
        {
            float v = ConstValue;
            if (Connect != null)
            {
                v += Connect.FValue;
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

        public void Add(object souce, float baseV, Porperty property, float scale)
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

            ReCalValue(child.Source);
        }

        public void Add(Porperty p)
        {
            Add(null, 0, p, 1);
        }

        public void Add(object souce, Porperty p, float scale = 1)
        {
            Add(souce, 0, p, scale);
        }

        public void Add(object souce, float constV, float scale = 1)
        {
            Add(souce, constV, null, scale);
        }

        public void Remove(object source)
        {
            Children.RemoveAll(ele => ele.Source == source);
            ReCalValue(source);
        }

        public override void ReCalValue(object source)
        {
            float v = OprationChild();

            SetNewValue(source, v);
        }

        /// <summary>
        /// 对子项进行运算
        /// </summary>
        /// <returns></returns>
        protected abstract float OprationChild();
    }

    public sealed class SumChildPopperty : CombinePoperty
    {
        protected override float OprationChild()
        {
            float v = 0;
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

            ReCalValue(null);
        }

        protected override float OprationChild()
        {
            float sum = 0;
            foreach (var item in Children)
            {
                sum += item.Cal();
            }
            var v = sum * (1 + LayerScale?.FValue ?? 0);
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
