using Megumin.GameFramework.ItemModule;
using Megumin.GameFramework.Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配置表读取一部分数据。
/// 所有加载完毕后，进如动态生成过程。
/// </summary>
public class ItemConfig : MonoBehaviour, IMeguminItemConfig
{
    public string Name;
    public int ID;
    public string Des;
    public int IconID;

    public string Description => Des;
    public string FriendlyName => Name;
    string IName.Name => Name;

    /// <summary>
    /// 动态生成
    /// </summary>
    public void DynamicGeneration()
    {
        
    }
}
