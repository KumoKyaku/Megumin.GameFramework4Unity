using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item
{
    [CreateAssetMenu(menuName = nameof(ItemConfigSO))]
    public class ItemConfigSO : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; set; } = "New Item";

        public GameObject Prefab;

        [EditorButton(true)]
        public void TestCreate(Vector3 position)
        {
            GameObject collectibleItem = GameObject.Instantiate(Prefab, position, Quaternion.identity); 
        }
    }
}
