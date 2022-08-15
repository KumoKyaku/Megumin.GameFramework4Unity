using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin.GameFramework.Numerical
{
    /// <summary>
    /// 伤害配置
    /// </summary>
    public interface IPropertyChange : IPropertyConfig
    {
        float CalChangeValue(IPropertyFinder target = null, IPropertyFinder creator = null);
        float CalChangeValue(Func<int, IPropertyFinder> findFunc);
    }

    /// <summary>
    /// 加成配置
    /// </summary>
    public interface IPropertyAdd : IPropertyConfig
    {
        ChildProperty CreateChildProperty(IPropertyFinder target = null, IPropertyFinder creator = null);
    }

    /// <summary>
    /// 属性查找
    /// </summary>
    public interface IPropertyFinder
    {
        bool TryGetValue(string type, out float fValue);
        bool TryGetPorperty(string type, out Porperty porperty);
    }
}
