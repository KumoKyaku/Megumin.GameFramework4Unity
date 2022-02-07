using Megumin.GameFramework.Item.SoP  ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item.SoP
{
    public class SoPItemFactory : ScriptableObject
    {
        internal SoPItem CreateItem(ItemConfigSO itemConfigSO)
        {
            var res = new SoPItem(itemConfigSO, Guid.NewGuid());
            return res;
        }
    }
}
