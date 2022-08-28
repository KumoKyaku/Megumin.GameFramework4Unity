using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework
{
    public interface IMetaGUIDable
    {
        string MetaGUID { get; set; }
    }

    public interface ITypeIDable<T>
        where T : struct
    {
        T TypeID { get; set; }
    }

    [SerializeField]
    public class ConfigHeader : IMetaGUIDable
    {
        [field: SerializeField]
        [field: MetaGUID]
        public string MetaGUID { get; set; }

        [field: TextArea(2, 15)]
        [field: SerializeField]
        public string Description { get; set; }

        /// <summary>
        /// 实质,灵感来源等,开发者备注
        /// </summary>
        [field: TextArea(1, 15)]
        [field: SerializeField]
        public string DeveloperNotes { get; set; }
    }

    public class ConfigSO : ScriptableObject, IMetaGUIDable, ITypeIDable<long>
    {
        [field: SerializeField]
        [field: MetaGUID]
        public string MetaGUID { get; set; }

        [field: TextArea(2, 15)]
        [field: SerializeField]
        public string Description { get; set; }

        /// <summary>
        /// 实质,灵感来源等,开发者备注
        /// </summary>
        [field: TextArea(1, 15)]
        [field: SerializeField]
        public string DeveloperNotes { get; set; }

        /// <summary>
        /// 绝对唯一不能重复
        /// </summary>
        [field: SerializeField]
        [field: Space]
        [field: ProtectedInInspector]
        public long TypeID { get; set; } = 10000000;

        [field: SerializeField]
        public Sprite Icon { get; set; }

        public virtual string Name => name;
    }
}
