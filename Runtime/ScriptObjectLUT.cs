using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Megumin.GameFramework
{
    public abstract class ScriptObjectLUT<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        [field: SerializeField]
        [field: Path]
        protected List<string> CollectFolder { get; set; } = new List<string>()
        {
            "Assets", "Packages"
        };

        [Space]
        [ReadOnlyInInspector]
        [SerializeField]
        private List<T> AllConfig = new List<T>();

        [ReadOnlyInInspector]
        [SerializeField]
        protected List<string> AllGuid = new List<string>();
        [ReadOnlyInInspector]
        [SerializeField]
        protected List<long> AllTypeID = new List<long>();

        public static Dictionary<string, T> GuidDic { get; } = new Dictionary<string, T>();
        public static Dictionary<long, T> TypeIDDic { get; } = new Dictionary<long, T>();

        public virtual void OnBeforeSerialize()
        {

        }

        public virtual void OnAfterDeserialize()
        {
            for (int i = 0; i < AllGuid.Count; i++)
            {
                GuidDic[AllGuid[i]] = AllConfig[i];
            }

            for (int i = 0; i < AllTypeID.Count; i++)
            {
                TypeIDDic[AllTypeID[i]] = AllConfig[i];
            }
        }

        [Button(order = -100)]
        public void CollectAll()
        {
#if UNITY_EDITOR

            var FindAssetsFolders = new string[] { "Assets", "Packages" };
            if (CollectFolder != null)
            {
                FindAssetsFolders = CollectFolder.ToArray();
            }

            string[] GUIDs = AssetDatabase.FindAssets($"t:{typeof(T).Name}",
                FindAssetsFolders);

            long max = 10000000;
            if (TypeIDDic.Count > 0)
            {
                max = TypeIDDic.Keys.Max();
            }
            AllConfig.Clear();
            AllGuid.Clear();
            AllTypeID.Clear();

            //第一遍循环检查ID冲突
            for (int i = 0; i < GUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
                var so = AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));

                if (so is IMetaGUIDable metaGUIDable)
                {
                    if (metaGUIDable.MetaGUID != GUIDs[i])
                    {
                        metaGUIDable.MetaGUID = GUIDs[i];
                        EditorUtility.SetDirty(so);
                    }
                }

                if (so is ITypeIDable<long> typeIDable)
                {
                    if (typeIDable.TypeID <= 10000000)
                    {
                        //统一分配TypeID.
                        max++;
                        typeIDable.TypeID = max;
                        EditorUtility.SetDirty(so);
                    }

                    if (TypeIDDic.TryGetValue(typeIDable.TypeID, out var cso))
                    {
                        if (cso is ITypeIDable<long> csotypeIDable && cso is UnityEngine.Object cuo)
                        {
                            if (typeIDable != csotypeIDable)
                            {
                                //类型ID冲突，将后创建的重新分配类型ID。
                                var thisCtime = so.GetCreationTimeUtc();
                                var tarCTime = cuo.GetCreationTimeUtc();
                                if (thisCtime > tarCTime)
                                {
                                    //重新分配TypeID.
                                    max++;
                                    typeIDable.TypeID = max;
                                    EditorUtility.SetDirty(so);
                                }
                                else
                                {
                                    //重新分配TypeID.
                                    max++;
                                    csotypeIDable.TypeID = max;
                                    EditorUtility.SetDirty(cuo);
                                    TypeIDDic[csotypeIDable.TypeID] = cso;
                                }
                            }
                        }

                    }

                    if (so is T tso)
                    {
                        TypeIDDic[typeIDable.TypeID] = tso;
                    }
                }
            }

            //第二遍循环添加到配置
            for (int i = 0; i < GUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
                var so = AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));

                if (so is T tso)
                {
                    AllConfig.Add(tso);
                    AllGuid.Add(GUIDs[i]);
                    GuidDic[GUIDs[i]] = tso;
                }

                if (so is ITypeIDable<long> typeIDable)
                {
                    AllTypeID.Add(typeIDable.TypeID);
                }
            }

            EditorUtility.SetDirty(this);
#endif
        }

        [Button(order = -99)]
        public void Clear()
        {
            AllConfig.Clear();
            AllGuid.Clear();
            AllTypeID.Clear();
            GuidDic.Clear();
            TypeIDDic.Clear();
        }
    }
}
