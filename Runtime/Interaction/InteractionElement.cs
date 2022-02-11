using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Interaction
{
    public class InteractionElement : MonoBehaviour, IInteractionElement
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public object InteractionObject { get; }

        public void PreInteract()
        {
            
        }

        public void PostInteract<T>(T result = default)
        {
            
        }
    }
}
