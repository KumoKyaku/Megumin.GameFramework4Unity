using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Interaction
{
    /// <summary>
    /// View≤„
    /// </summary>
    public interface IInteractionElement
    {
        object InteractionObject { get; }
    }

    /// <summary>
    /// Ωªª•∂‘œÛData
    /// </summary>
    public interface IInteractionObject
    {
        void Interact(object sender,object option = default);
    }
}