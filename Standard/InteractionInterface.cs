using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Interaction
{
    /// <summary>
    /// View层
    /// </summary>
    public interface IInteractionElement
    {
        object InteractionObject { get; }
        void PreInteract();
        void PostInteract<T>(T result = default);
    }

    /// <summary>
    /// 交互对象Data 设计有误
    /// </summary>
    [System.Obsolete]
    public interface IInteractionObject
    {
        void Interact(object sender,object option = default);
    }
}