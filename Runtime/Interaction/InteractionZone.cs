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
		public UnityEvent<bool, GameObject> OnTrigger = default;

		private void OnTriggerEnter(Collider other)
		{
			if ((1 << other.gameObject.layer & MaskLayer) != 0)
			{
				OnTrigger?.Invoke(true, other.gameObject);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if ((1 << other.gameObject.layer & MaskLayer) != 0)
			{
				OnTrigger?.Invoke(false, other.gameObject);
			}
		}
	}
}
