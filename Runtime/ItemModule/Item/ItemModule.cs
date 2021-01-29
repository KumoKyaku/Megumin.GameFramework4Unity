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

        public ItemModule(CP configProvider)
        {
            ConfigProvider = configProvider;
        }

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

    public class TestItemModule : ItemModule<ItemConfigProvider, ItemConfig, Item>
    {
        public TestItemModule(ItemConfigProvider configProvider) : base(configProvider)
        {
            ConfigProvider = configProvider;
        }

        public static TestItemModule Current { get; set; }

        /// <summary>
        /// 添加临时测试物品
        /// </summary>
        public void FakeCreateMyItem()
        {
            System.Random random = new System.Random();
            int i = 0;
            foreach (var cfg in ConfigProvider.itemConfigs)
            {
                var item = new Item(cfg)
                {
                    ItemInstanceID = i,
                    Count = random.Next(1, 100),
                };

                MyItems.Add(item.ItemInstanceID, item);
                i++;
            }
        }
    }
}
