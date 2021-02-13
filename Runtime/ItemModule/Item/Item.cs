using Megumin.GameFramework.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin.GameFramework.ItemModule
{
    /// <summary>
    /// 约束集合
    /// </summary>
    public interface IMeguminItemConfig : IName, IFriendlyName, IDescribable, IIcon
    {

    }

    /// <summary>
    /// 约束集合
    /// </summary>
    public interface IMeguminItem : IMeguminItemConfig
    {

    }

    /// <summary>
    /// 用于任务物品显示
    /// <para>不是配置，因为可以指示数量；也不是真实物品</para>
    /// </summary>
    public partial class Item<CFG> : IMeguminItem
        where CFG : IMeguminItemConfig
    {
        public CFG Config { get; protected set; }

        public string Name => Config.Name;

        public string FriendlyName => Config.FriendlyName;

        public string Description => Config.Description;
    }

}
