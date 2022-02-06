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
        }

        private LinkedList<Interaction> _potentialInteractions = new LinkedList<Interaction>(); //To store the objects we the player could potentially interact with

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

        private void AddPotentialInteraction(GameObject obj)
        {
            Interaction newPotentialInteraction = new Interaction();
            newPotentialInteraction.interactableObject = obj;
            newPotentialInteraction.type = obj.tag;

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
