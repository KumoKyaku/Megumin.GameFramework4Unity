using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Standard
{
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
        public string GetName<T>(T option = default)
        {
            return Name;
        }
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

