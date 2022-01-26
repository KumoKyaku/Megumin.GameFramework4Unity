using Megumin.GameFramework.Standard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item
{
    /// <summary>
    /// 用于任务物品显示
    /// <para>不是配置，因为可以指示数量；也不是真实物品</para>
    /// </summary>
    public partial class ItemTemplate<CFG>
        where CFG : IMeguminItemConfig
    {
        public ItemTemplate(CFG cfg)
        {
            this.Config = cfg;
        }

        public CFG Config { get; protected set; }
    }

    public class VirsualItem
    {

    }
}

