using Megumin.GameFramework.Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item.SoP  
{
    /// <summary>
    /// 用于任务物品显示
    /// <para>不是配置，因为可以指示数量；也不是真实物品</para>
    /// </summary>
    public partial class SoPItemBase<CFG> : Item<CFG>
        where CFG : IName, IFriendlyName, IDescribable
    {
        public string Name => Config.Name;

        public string FriendlyName => Config.FriendlyName;

        public string Description => Config.Description;
    }

    public class SoPItem : SoPItemBase<ItemConfigSO>
    {
        public SoPItem(ItemConfigSO itemConfigSO, Guid guid)
        {
            Config = itemConfigSO;
            GUID = guid;
        }

        public Guid GUID { get; protected set; } = Guid.Empty;
        public bool IsInstance => Guid.Empty != GUID;
    }
}
