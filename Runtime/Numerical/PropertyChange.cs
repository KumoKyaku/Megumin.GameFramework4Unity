using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Megumin.GameFramework.Numerical
{
    /// <summary>
    /// ���Ը���
    /// </summary>
    public interface IPropertyChange 
    {
        string Type { get; }

        double CalChangeValue(INumericalPropertyFinder2 numericalActor1, INumericalPropertyFinder2 numericalActor2 = null);
        double CalChangeValue(Func<int, INumericalPropertyFinder2> findFunc);
    }

    /// <summary>
    /// �������Լӳ�
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
