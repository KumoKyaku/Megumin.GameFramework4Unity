using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Megumin.GameFramework.Interaction
{
    public class UIInteractionEventSO : ScriptableObject
    {
        public UnityAction<bool, Interaction> OnEventRaised;

        internal void RaiseEvent(bool visible, LinkedListNode<Interaction> first)
        {
            OnEventRaised?.Invoke(visible, first?.Value);
        }
    }
}
