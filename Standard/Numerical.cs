using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///数值模块 标准接口
namespace Megumin.GameFramework.Numerical
{
    /// <summary>
    /// 数值属性的最基本功能
    /// <para>支持观察者模式</para>
    /// </summary>
    public interface IProperty : IObservable<int>
    {
        /// <summary>
        /// 当前值
        /// </summary>
        int Value { get; }
        event OnValueChanged<int> ValueChange;
    }

    /// <summary>
    /// <inheritdoc/>
    /// 具有最后修改数据源的属性
    /// </summary>
    public interface IPropertyWithSource : IProperty, IObservable<(object Source, int Value)>
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
