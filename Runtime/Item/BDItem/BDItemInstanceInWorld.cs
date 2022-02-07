using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin.GameFramework;
using Megumin.GameFramework.Interaction;

namespace Megumin.GameFramework.Item.BD
{
    public class BDItemInstanceInWorld : ItemInstanceInWorld, 
        IInteractionElement
    {
        public bool CreateInstance = false;
        public BDItemFactory BDItemFactory;
        public BDItem BDItem;

        private void Awake()
        {

        }

        private void Start()
        {
            if (CreateInstance)
            {
                BDItem = BDItemFactory.CreateItem(ItemConfigSO);
            }
            
        }

        [OnF1]
        [EditorButton]
        public void LogItem()
        {
            Debug.Log(BDItem.ToStringReflection());
        }

        private void OnDrawGizmos()
        {
            //if (ItemConfigSO && ItemConfigSO.Icon)
            //{
            //    ItemConfigSO.Icon.GizmoDraw(transform.position);
            //}
        }

        public void OnF1(object option = null)
        {
            LogItem();
        }
    }
}
