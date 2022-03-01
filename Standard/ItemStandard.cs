using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Standard
{

    public interface IGrayable
    {
        bool IsGray { get; }

        public bool GetGrayState<T>(T option = default)
        {
            return IsGray;
        }
    }

    /// <summary>
    /// 可堆叠的
    /// </summary>
    public interface IStackable
    {
        int MaxStackCount { get; }
        int StackCount { get; }
    }

    public interface INameable
    {
        string Name { get; }

        /// <summary>
        /// 接口 泛型导致IL2CPP崩溃?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <remarks>怀疑错了, 接口 + 泛型 + 接口默认实现方法 + 可选参数 + IL2CPP，都没崩,可以按照预期工作。</remarks>
        //[UnityEngine.Scripting.Preserve]
        public string GetName<T>(T option = default)
        {
#if MEGUMIN_DEBUG_IL2CPP
            Debug.Log($"INameable GetName T:{typeof(T).Name} {Name}");
#endif
            return Name;
        }

        //public string GetName(string option = default)
        //{
        //    return Name;
        //}
    }

    public interface IDisplayNameable
    {
        string DisplayName { get; }
    }

    public interface IIndexable<out T>
    {
        T Index { get; }
    }

    public interface IFriendlyName
    {
        string FriendlyName { get; }
    }

    public interface IDescribable
    {
        string Description { get; }
    }
}

