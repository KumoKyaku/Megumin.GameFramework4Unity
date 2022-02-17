using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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
        public List<T> Items = new List<T>();
        public HashSet<T> ItemHashSet = new HashSet<T>();
        /// <summary>
        /// 可能需要网络验证
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual Task<int> AddNewItem(T item,bool autoMerge = true)
        {
            if (ItemHashSet.Add(item))
            {
                Items.Add(item);
            }

            return Task.FromResult(0);
        }

        public virtual bool IsIn(T item)
        {
            return false;
        }
    }

    
}
