using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin.GameFramework.Numerical
{
    /// <summary>
    /// 属性更改
    /// </summary>
    public interface IPropertyChange 
    {
        string Type { get; }

        double CalChangeValue(INumericalPropertyFinder2 numericalActor1, INumericalPropertyFinder2 numericalActor2 = null);
        double CalChangeValue(Func<int, INumericalPropertyFinder2> findFunc);
    }

    /// <summary>
    /// 用于属性加成
    /// </summary>
    public interface IPropertyAdd
    {

    }

    public interface INumericalPropertyFinder2
    {
        bool TryGetValue(string type, out double dValue);
    }

    public class ChangeValueResult
    {
        public string Type { get;set; }
        public double Value { get; set; }
    }
}
