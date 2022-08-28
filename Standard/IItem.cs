using UnityEngine;

namespace Megumin.GameFramework.Item
{
    public interface IItemConfig
    {
        string DisplayName { get; set; }
        string FriendlyName { get; set; }
        int MaxStackCount { get; set; }
        GameObject ModelPrefab { get; set; }
        GameObject Prefab { get; set; }

        GameObject CreateItemInWorld(Vector3 position, HideFlags hideFlags = default);
        void TestCreate(Vector3 position);
    }
}


