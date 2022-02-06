using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Interaction
{
    public class InteractionListener : MonoBehaviour
    {
        public class Interaction
        {
            public string type;
            public GameObject interactableObject;
            public IInteractionElement Compment;
        }

        public LinkedList<Interaction> _potentialInteractions = new LinkedList<Interaction>(); //To store the objects we the player could potentially interact with

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

            RequestUpdateUI(_potentialInteractions.Count > 0);
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
        }
    }
}
