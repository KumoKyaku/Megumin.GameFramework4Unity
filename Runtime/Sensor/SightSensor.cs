using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public partial class SightSensor : Sensor
    {
        [ReadOnlyInInspector]
        public string Type = SensorType.Sight;

        [Range(0, 30)]
        public float Radius = 15;
        [Range(0, 360)]
        public float Angle = 90;
        /// <summary>
        /// Todo,暂时
        /// </summary>
        [Range(0, 180)]
        public float FOV = 60;

        private void Start()
        {
            
        }

        /// <summary>
        /// 暂时只做了水平测试
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Check(MonoBehaviour target, Collider item)
        {
            if (!enabled)
            {
                return false;
            }

            //一个对象可能有多个碰撞盒，每个碰撞盒都要检测
            var dis = Vector3.Distance(transform.position, target.transform.position);
            if (dis < Radius)
            {
                var deltaAngle = Quaternion.Angle(target.transform.rotation, transform.rotation);
                if (deltaAngle < Angle)
                {
                    //在视觉范围内
                    return true;
                }
            }
            return false;
        }

        [Header("Debug")]
        [Space]
        public bool DebugSolid = false;
        public Color DebugColor = Color.green;
        [Range(0, 10)]
        public float DebugLineThickness = 2;
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
            if (DebugSolid)
            {
                Handles.DrawSolidArc(transform.position, transform.up,
                                transform.forward,
                                Angle / 2, Radius);
                Handles.DrawSolidArc(transform.position, transform.up * -1,
                    transform.forward,
                    Angle / 2, Radius);
            }

            var wireColor = Handles.color;
            wireColor.a = 1;
            Handles.color = wireColor;

            var leftDir = Quaternion.AngleAxis(-Angle / 2, transform.up);
            var left = leftDir * transform.forward * Radius;
            Handles.DrawLine(transform.position, transform.position + left, DebugLineThickness);

            var rightDir = Quaternion.AngleAxis(Angle / 2, transform.up);
            var right = rightDir * transform.forward * Radius;
            Handles.DrawLine(transform.position, transform.position + right, DebugLineThickness);

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


