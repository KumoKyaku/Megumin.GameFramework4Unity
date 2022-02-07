using Megumin.GameFramework.Item;
using Megumin.GameFramework.Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item.Test
{
    /// <summary>
    /// 配置表读取一部分数据。
    /// 所有加载完毕后，进如动态生成过程。
    /// </summary>
    public class ItemConfig : MonoBehaviour
    {
        public string Name;
        public int ID;
        public string Des;
        public int IconID;

        public string Description => Des;
        public string FriendlyName => Name;

        /// <summary>
        /// 动态生成
        /// </summary>
        public void DynamicGeneration()
        {

        }
    }

}
