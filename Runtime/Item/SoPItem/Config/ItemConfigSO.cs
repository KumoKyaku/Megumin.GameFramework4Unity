using Megumin.GameFramework.Standard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item
{
    [CreateAssetMenu(menuName = nameof(ItemConfigSO))]
    public class ItemConfigSO : ScriptableObject,
        IName, IFriendlyName, IDescribable
    {
        [field: SerializeField]
        public string Name { get; set; } = "New Item";
        public string FriendlyName { get; set; }
        [field: SerializeField]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite Icon { get; set; }    

        public GameObject Prefab;
        public GameObject ModelPrefab;

        [EditorButton(true)]
        public void TestCreate(Vector3 position)
        {
            GameObject collectibleItem = GameObject.Instantiate(Prefab, position, Quaternion.identity); 
        }

        
    }
}
