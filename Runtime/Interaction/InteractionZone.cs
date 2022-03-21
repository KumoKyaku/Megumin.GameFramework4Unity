using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Megumin.GameFramework.Interaction
{
    /// <summary>
    /// 交互触发区域
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

        //public HashSet<Collider> InZone = new HashSet<Collider>();

        [ReadOnlyInInspector]
        public List<Collider> Colliders;
        private void Awake()
        {
            gameObject.GetComponentsInChildren<Collider>(Colliders);
            foreach (var item in Colliders)
            {
                if (!item.isTrigger)
                {
                    Debug.LogWarning($"碰撞和应该是trigger，{item.name}");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!CheckMask(other))
            {
                return;
            }

            //InZone.Add(other);
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

        public bool CheckMask(Collider other)
        {
            if ((1 << other.gameObject.layer & MaskLayer) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public void Update()
        //{
        //    using(ListPool<Collider>.Rent(out var list))
        //    {
        //        //碰撞盒销毁不会除非Exit,手动检测是否被销毁.
        //        //other.gameObject 已经销毁,无法实现.改为在listener中处理.
        //        foreach (var item in InZone)
        //        {
        //            if (item)
        //            {

        //            }
        //            else
        //            {
        //                list.Add(item);
        //            }
        //        }

        //        foreach (var item in list)
        //        {
        //            OnTriggerExit(item);
        //        }
        //    }
        //}

        /// <summary>
        /// 碰撞盒销毁不会除非Exit
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (!CheckMask(other))
            {
                return;
            }

            //InZone.Remove(other);
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
