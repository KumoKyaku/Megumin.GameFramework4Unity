using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeguminEngine.Base
{
    public class ItemModule
    {
        private ItemConfigProvider configProvider;

        public ItemModule(ItemConfigProvider configProvider)
        {
            this.configProvider = configProvider;
            Init();
        }

        public static ItemModule Current { get; set; }

        public Dictionary<long, Item> MyItems { get; } = new Dictionary<long, Item>();
        internal void Init()
        {
            Random random = new Random();
            //Fake
            int i = 0;
            foreach (var cfg in configProvider.itemConfigs)
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
