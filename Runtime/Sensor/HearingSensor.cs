using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public partial class HearingSensor : Sensor
    {
        [Range(0, 30)]
        public float Radius = 10;

        public bool Check(Transform target)
        {
            var dis = Vector3.Distance(transform.position, target.transform.position);
            if (dis < Radius)
            {
                //ÔÚÊÓÌý¾õÎ§ÄÚ
                return true;
            }
            return false;
        }

        [Header("Debug")]
        [Space]
        public Color DebugColor = Color.green;
        [Range(0, 20)]
        public float DebugLineThickness = 5;
    }
}


#if UNITY_EDITOR

namespace Megumin.GameFramework.Sensor
{
    using UnityEditor;
    partial class HearingSensor
    {
        private void OnDrawGizmosSelected()
        {
            //»æÖÆÌý¾õ°ë¾¶
            Gizmos.color = DebugColor;
            Gizmos.DrawSphere(transform.position, Radius);
            var wireColor = Gizmos.color;
            wireColor.a = 1;
            Gizmos.color = wireColor;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}

#endif

