using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Inventory
{
    /// <summary>
    /// 背包和物品应该弱相关.
    /// <para/>背包只处理背包元素,因为物品实现了背包元素,所以背包可以处理物品.
    /// </summary>
    public class Inventory
    {
        public int SoltCount { get; set; }
        public List<IInventoryElement> Elements { get; set; }
            = new List<IInventoryElement>();

        public void ReadFromSaver()
        {

        }
    }

    public interface IInventoryElement
    {

    }

    public class Inventory<T> : Inventory
        where T : IInventoryElement
    {

    }

    /// <summary>
    /// 保存器接口,可以是网络保存器,也可以是本地保存器
    /// <para>一些操作如拆分,需要保存器确认</para>
    /// 也应该可以通过保存器填充背包,刷新背包
    /// </summary>
    public interface ISaver
    {

    }
}
