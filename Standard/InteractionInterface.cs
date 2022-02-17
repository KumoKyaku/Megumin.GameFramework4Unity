using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin.GameFramework.Interaction
{
    /// <summary>
    /// 可被交互的对象的View层,负责触发碰撞盒,保存在待交互缓存区
    /// </summary>
    public interface IInteractionElement
    {
        object InteractionObject { get; }
        void PreInteract();
        void PostInteract<T>(T result = default);
    }

    /// <summary>
    /// 可被交互的
    /// </summary>
    public interface IInteractable
    {
        Task<int> Interact(object sender, object option = null);
    }

    /// <summary>
    /// 交互对象Data 设计有误
    /// </summary>
    [System.Obsolete]
    public interface IInteractionObject
    {
        void Interact(object sender, object option = default);
    }
}



