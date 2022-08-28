using Megumin;
using Megumin.GameFramework.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Item
{
    public abstract class ItemSpawer : MonoBehaviour
    {
        public HideFlags HideFlags = HideFlags.DontSave | HideFlags.DontSaveInEditor | HideFlags.NotEditable
                            | HideFlags.DontSaveInBuild;

        public abstract IItemConfig ItemConfig { get; }
        public bool SpawOnStart = true;

        private void Start()
        {
            if (SpawOnStart)
            {
                Spaw();
            }
        }

        [Button]
        public void Spaw()
        {
            Spaw(ItemConfig);
        }

        public void Spaw(IItemConfig config)
        {
            if (config != null)
            {
                var tar = config.CreateItemInWorld(transform.position, HideFlags);
                if (tar)
                {
                    tar.transform.SetParent(transform);
                    tar.SetLayerOnAll(gameObject.layer);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (ItemConfig != null && ItemConfig.ModelPrefab)
            {
                var meshFilter = ItemConfig.ModelPrefab.GetComponent<MeshFilter>();
                if (meshFilter)
                {
                    Gizmos.DrawWireMesh(
                        meshFilter.sharedMesh, 0,
                        transform.position + Vector3.up * 0.5f,
                        meshFilter.transform.rotation,
                        meshFilter.transform.lossyScale);
                }

            }
        }
    }
}

