using Megumin.GameFramework.Standard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.ItemModule
{
    /// <summary>
    /// ����������Ʒ��ʾ
    /// <para>�������ã���Ϊ����ָʾ������Ҳ������ʵ��Ʒ</para>
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
}

