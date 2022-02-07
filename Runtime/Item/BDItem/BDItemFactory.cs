using Megumin.GameFramework.Item.BD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item
{
    public class BDItemFactory : ScriptableObject
    {
        internal BDItem CreateItem(ItemConfigSO itemConfigSO)
        {
            var res = new BDItem(itemConfigSO, Guid.NewGuid());
            return res;
        }
    }
}
