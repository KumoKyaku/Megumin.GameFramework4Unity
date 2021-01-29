using MeguminEngine.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeguminEngine.Base
{
    public class Item: IName, IFriendlyName, IDescribable
    {
        private ItemConfig cfg;

        public Item(ItemConfig cfg)
        {
            this.cfg = cfg;
        }

        public long ItemInstanceID { get; internal set; }
        public int Count { get; internal set; }


        public string GetProp(int v)
        {
            return $"HP +{Count}";
        }

        public string Name => ((IName)cfg).Name;

        public string FriendlyName => ((IFriendlyName)cfg).FriendlyName;

        public string Description => ((IDescribable)cfg).Description;
    }
}
