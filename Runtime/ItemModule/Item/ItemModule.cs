using Megumin.GameFramework.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin.GameFramework.ItemModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="CP"></typeparam>
    public class ItemModule<CP, ItemConfig, Item>
        where CP : IConfigProvider<ItemConfig>
        where ItemConfig : IMeguminItemConfig
        where Item : IMeguminItem
    {
        public CP ConfigProvider { get; set; }

        public void LogAllConfig()
        {
            string config = "Config:    ";
            foreach (var item in ConfigProvider.Config)
            {
                config += $"\n {item.Name} | {item.FriendlyName}";
            }

            Debug.Log(config);
        }

        /// <summary>
        /// 物品实例
        /// </summary>
        public Dictionary<long, Item> MyItems { get; } = new Dictionary<long, Item>();

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual ItemTemplate<ItemConfig> CreateTemplate(ItemConfig config)
        {
            return new ItemTemplate<ItemConfig>(config);
        }
    }

}
