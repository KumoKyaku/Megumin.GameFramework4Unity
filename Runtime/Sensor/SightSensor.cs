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
        public float HorizontalAngle = 120;
        [Range(0, 360)]
        public float VerticalAngle = 160;


        private void Start()
        {

        }

        public void Update()
        {
            if (Time.time < nextCheckStamp)
            {
                return;
            }
            nextCheckStamp = Time.time + checkDelta;

            if (PhysicsTestRadiusSelf)
            {
                var collidersInRadius = Physics.OverlapSphere(transform.position, Radius);
                foreach (var item in collidersInRadius)
                {
                    CheckTarget(item);
                }
            }
        }

        public virtual bool CheckTarget(Collider target)
        {
            return false;
        }
        
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
                var dir = target.transform.position - transform.position;
                var hAngle = Vector3.SignedAngle(dir, transform.forward, transform.up);
                hAngle = Mathf.Abs(hAngle);
                if (hAngle > HorizontalAngle / 2)
                {
                    return false;
                }

                var vAngle = Vector3.SignedAngle(dir, transform.forward, transform.right);
                vAngle = Mathf.Abs(vAngle);
                if (vAngle > VerticalAngle / 2)
                {
                    return false;
                }

                return true;
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
            if (!enabled || !GlobalDebugShow)
            {
                return;
            }

            DrawArc(transform.up, transform.forward, HorizontalAngle);
            DrawArc(transform.right, transform.forward, VerticalAngle);
        }

        public void DrawArc(Vector3 axis, Vector3 forward, float angle)
        {
            Handles.color = DebugColor;
            if (DebugSolid)
            {
                Handles.DrawSolidArc(transform.position, axis,
                                forward,
                                angle / 2, Radius);
                Handles.DrawSolidArc(transform.position, axis * -1,
                    forward,
                    angle / 2, Radius);
            }

            var wireColor = Handles.color;
            wireColor.a = 1;
            Handles.color = wireColor;

            var leftDir = Quaternion.AngleAxis(-angle / 2, axis);
            var left = leftDir * forward * Radius;
            Handles.DrawLine(transform.position, transform.position + left, DebugLineThickness);

            var rightDir = Quaternion.AngleAxis(angle / 2, axis);
            var right = rightDir * forward * Radius;
            Handles.DrawLine(transform.position, transform.position + right, DebugLineThickness);

            Handles.DrawWireArc(transform.position, axis,
                forward,
                angle / 2, Radius, DebugLineThickness);
            Handles.DrawWireArc(transform.position, axis * -1,
                forward,
                angle / 2, Radius, DebugLineThickness);
        }
    }
}

#endif


