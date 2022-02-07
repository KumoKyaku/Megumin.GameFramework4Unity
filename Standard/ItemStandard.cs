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
    }

    public interface IName
    {
        string Name { get; }
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

