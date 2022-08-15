using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///数值模块 标准接口
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

    public interface ISetValueable<in T>
    {
        void SetValue(T Value);
        void SetValue(object source, T Value);
    }

    public interface IPropertyConfig<out T>
    {
        T Type { get; }
    }

    public interface IPropertyConfig : IPropertyConfig<string>
    {
    }
}
