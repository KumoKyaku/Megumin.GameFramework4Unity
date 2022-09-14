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
    public interface IProperty<out V> : IObservable<V>
    {
        /// <summary>
        /// 当前值，用于计算，防止累计误差。
        /// </summary>
        V Value { get; }
        event OnValueChanged<V> ValueChanged;
    }

    /// <summary>
    /// <inheritdoc/>
    /// 具有最后修改数据源的属性
    /// </summary>
    public interface IPropertyWithSource<S, V> : IProperty<V>, IObservable<(S Source, V Value)>
    {
        /// <summary>
        /// 最后数值变动的原因
        /// </summary>
        S Source { get; }
        event OnValueChanged<(S Source, V Value)> SourceValueChanged;
    }

    /// <summary>
    /// 数值属性的最基本功能
    /// <para>支持观察者模式</para>
    /// </summary>
    public interface IProperty : IProperty<float>
    {
        /// <summary>
        /// 当前值，用于UI，或者粗略判断等，不用每次从float转型。
        /// </summary>
        int IntValue { get; }
    }

    /// <summary>
    /// <inheritdoc/>
    /// 具有最后修改数据源的属性
    /// </summary>
    public interface IPropertyWithSource : IProperty, IPropertyWithSource<object, float>
    {
    }

    public interface ISetValueable<in V>
    {
        void SetValue(V Value);
        void SetValue(object source, V Value);
    }

    public interface IPropertyConfig<out T>
    {
        T Type { get; }
    }

    public interface IPropertyConfig : IPropertyConfig<string>
    {
    }
}
