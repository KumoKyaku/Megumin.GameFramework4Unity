using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Provider",menuName = "ItemConfig")]
public class ItemConfigProvider : ScriptableObject,ISerializationCallbackReceiver
{
    public List<ItemConfig> itemConfigs;

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        //加载过程
        //解析过程

        foreach (var item in itemConfigs)
        {
            item.DynamicGeneration();
        }
    }
}
