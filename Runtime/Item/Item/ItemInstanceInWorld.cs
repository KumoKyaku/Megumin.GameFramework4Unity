using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item
{
    public interface IItemInstance
    {

    }

    public class ItemInstanceInWorld : MonoBehaviour, IItemInstance
    {
        public ItemConfigSO ItemConfigSO;
        public GameObject ModelRoot;
    }
}
