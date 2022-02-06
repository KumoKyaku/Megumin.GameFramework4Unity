using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Megumin.GameFramework.Interaction
{
    /// <summary>
    /// ½»»¥´¥·¢ÇøÓò
    /// </summary>
    public class InteractionZone : MonoBehaviour
    {
        public LayerMask MaskLayer = -1;

        [Space]
        public bool GameObjectTrigger = false;
        public UnityEvent<bool, GameObject> OnGameObjectTrigger = default;

        public bool ElementTrigger = true;
        public bool AlsoFindParentElement = false;
        public UnityEvent<bool, GameObject, IInteractionElement> OnElementTrigger = default;

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer & MaskLayer) != 0)
            {

            }
            else
            {
                return;
            }

            if (GameObjectTrigger)
            {
                OnGameObjectTrigger?.Invoke(true, other.gameObject);
            }

            if (ElementTrigger)
            {
                IInteractionElement comp = null;
                if (AlsoFindParentElement)
                {
                    comp = other.GetComponentInParent<IInteractionElement>();
                }
                else
                {
                    comp = other.GetComponent<IInteractionElement>();
                }

                if (comp is Behaviour behaviour && behaviour && behaviour.enabled)
                {
                    OnElementTrigger?.Invoke(true, behaviour.gameObject, comp);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((1 << other.gameObject.layer & MaskLayer) != 0)
            {

            }
            else
            {
                return;
            }

            if (GameObjectTrigger)
            {
                OnGameObjectTrigger?.Invoke(false, other.gameObject);
            }

            if (ElementTrigger)
            {
                IInteractionElement comp = null;
                if (AlsoFindParentElement)
                {
                    comp = other.GetComponentInParent<IInteractionElement>();
                }
                else
                {
                    comp = other.GetComponent<IInteractionElement>();
                }

                if (comp is Behaviour behaviour && behaviour && behaviour.enabled)
                {
                    OnElementTrigger?.Invoke(false, behaviour.gameObject, comp);
                }
            }
        }
    }
}
