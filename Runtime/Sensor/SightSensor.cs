using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public partial class SightSensor : Sensor
    {
        [Range(0, 30)]
        public float Radius = 15;
        [Range(0, 360)]
        public float Angle = 90;
        [Range(0, 180)]
        public float FOV = 60;
        public bool Check(Transform target)
        {
            var dis = Vector3.Distance(transform.position, target.transform.position);
            if (dis < Radius)
            {
                var deltaAngle = Quaternion.Angle(target.transform.rotation, transform.rotation);
                if (deltaAngle < Angle)
                {
                    //ÔÚÊÓ¾õ·¶Î§ÄÚ
                    return true;
                }
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
    partial class SightSensor
    {
        private void OnDrawGizmosSelected()
        {
            Handles.color = DebugColor;
            Handles.DrawSolidArc(transform.position, transform.up,
                transform.forward,
                Angle / 2, Radius);
            Handles.DrawSolidArc(transform.position, transform.up * -1,
                transform.forward,
                Angle / 2, Radius);

            var wireColor = Handles.color;
            wireColor.a = 1;
            Handles.color = wireColor;
            Handles.DrawWireArc(transform.position, transform.up,
                transform.forward,
                Angle / 2, Radius, DebugLineThickness);
            Handles.DrawWireArc(transform.position, transform.up * -1,
                transform.forward,
                Angle / 2, Radius, DebugLineThickness);
        }
    }
}

#endif


