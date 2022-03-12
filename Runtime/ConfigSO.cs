using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework
{
    public class ConfigSO : ScriptableObject
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

        public string Name => name;
    }
}
