using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Interaction
{
    public class Interaction
    {
        public string type;
        public GameObject interactableObject;
        public IInteractionElement Compment;
    }

    public class InteractionListener : MonoBehaviour
    {
        public LinkedList<Interaction> _potentialInteractions = new LinkedList<Interaction>(); //To store the objects we the player could potentially interact with
        public UIInteractionEventSO uIInteractionEventSO;

        public bool HasValue => _potentialInteractions.Count > 0;

        /// <summary>
        /// Called by the Event on the trigger collider on the child GO called "InteractionZone"
        /// </summary>
        /// <param name="entered"></param>
        /// <param name="obj"></param>
        public void OnTriggerChangeDetected(bool entered, GameObject obj)
        {
            if (entered)
                AddPotentialInteraction(obj);
            else
                RemovePotentialInteraction(obj);
        }

        public void OnTriggerChangeDetected(bool entered, GameObject obj, IInteractionElement element)
        {
            if (entered)
                AddPotentialInteraction(obj, element);
            else
                RemovePotentialInteraction(obj);
        }

        public void Update()
        {
            using (ListPool<Interaction>.Rent(out var list))
            {
                //手动检测对象是否被销毁.
                foreach (var item in _potentialInteractions)
                {
                    if (item.interactableObject)
                    {

                    }
                    else
                    {
                        list.Add(item);
                    }
                }

                foreach (var item in list)
                {
                    RemoveInteraction(item);
                }
            }
        }

        private void RemovePotentialInteraction(GameObject obj)
        {
            LinkedListNode<Interaction> currentNode = _potentialInteractions.First;
            while (currentNode != null)
            {
                if (currentNode.Value.interactableObject == obj)
                {
                    _potentialInteractions.Remove(currentNode);
                    break;
                }
                currentNode = currentNode.Next;
            }

            RequestUpdateUI(HasValue);
        }

        private void RemoveInteraction(Interaction node)
        {
            _potentialInteractions.Remove(node);
            RequestUpdateUI(HasValue);
        }

        private void AddPotentialInteraction(GameObject obj, IInteractionElement element = null)
        {
            Interaction newPotentialInteraction = new Interaction();
            newPotentialInteraction.interactableObject = obj;
            newPotentialInteraction.type = obj.tag;
            newPotentialInteraction.Compment = element;

            if (true)
            {
                _potentialInteractions.AddFirst(newPotentialInteraction);
                RequestUpdateUI(true);
            }
        }

        private void RequestUpdateUI(bool visible)
        {
            //todo
            uIInteractionEventSO.RaiseEvent(visible, _potentialInteractions.First);
        }
    }
}
