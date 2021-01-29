using Megumin.GameFramework.Standard;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.ItemModule
{
    [CreateAssetMenu(fileName = "Provider", menuName = "ItemConfig")]
    public class ItemConfigProvider : ScriptableObject,
    ISerializationCallbackReceiver,
    IConfigProvider<ItemConfig>
    {
        public List<ItemConfig> itemConfigs;

        public List<ItemConfig> Config => itemConfigs;

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

    public class JsonItemConfigProvider : IConfigProvider<IMeguminItemConfig>
    {
        public List<IMeguminItemConfig> Config { get; } = new List<IMeguminItemConfig>();
        public JsonItemConfigProvider(string json)
        {
            //Parse
        }
    }

    public class ExcelItemConfigProvider : IConfigProvider<IMeguminItemConfig>
    {
        public List<IMeguminItemConfig> Config { get; } = new List<IMeguminItemConfig>();
        public ExcelItemConfigProvider(string path)
        {
            //Parse
        }
    }
}

