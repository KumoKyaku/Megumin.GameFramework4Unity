using Megumin.GameFramework.Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item.BD
{
    /// <summary>
    /// ����������Ʒ��ʾ
    /// <para>�������ã���Ϊ����ָʾ������Ҳ������ʵ��Ʒ</para>
    /// </summary>
    public partial class BDItemBase<CFG> : Item<CFG>
        where CFG : IName, IFriendlyName, IDescribable
    {
        public string Name => Config.Name;

        public string FriendlyName => Config.FriendlyName;

        public string Description => Config.Description;
    }

    public class BDItem : BDItemBase<ItemConfigSO>
    {
        public BDItem(ItemConfigSO itemConfigSO, Guid guid)
        {
            Config = itemConfigSO;
            GUID = guid;
        }

        public Guid GUID { get; protected set; } = Guid.Empty;
        public bool IsInstance => Guid.Empty != GUID;
    }
}
