using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public partial class AISensor : MonoBehaviour
    {
        [ProtectedInInspector]
        public HearingSensor HearingSensor;
        [ProtectedInInspector]
        public SightSensor SightSensor;

        [Range(0,5)]
        public float checkDelta = 0.5f;

        public void Awake()
        {
            FindC();
        }

        private void Reset()
        {
            FindC();
        }

        void FindC()
        {
            if (!HearingSensor)
            {
                HearingSensor = GetComponentInChildren<HearingSensor>();
            }

            if (!SightSensor)
            {
                SightSensor = GetComponentInChildren<SightSensor>();
            }
        }

        public List<SensorTarget> SightTarget = new List<SensorTarget>();
        public List<SensorTarget> HearingTarget = new List<SensorTarget>();
        
        private float nextCheckStamp;

        private void Update()
        {
            if (Time.time + checkDelta > nextCheckStamp)
            {
                return;
            }
            nextCheckStamp = Time.time + checkDelta;

            var mixR = Mathf.Max(HearingSensor.Radius, SightSensor.Radius);

            var collidersInRadius =
                Physics.OverlapSphere(transform.position, mixR);

            //HashSet<SensorTarget> dealed = new HashSet<SensorTarget>();
            foreach (var item in collidersInRadius)
            {
                var tarC = item.GetComponentInParent<SensorTarget>();
                if (!tarC)
                {
                    continue;
                }

                if (SightSensor.Check(tarC.transform))
                {
                    //在视觉范围内
                    SightTarget.Add(tarC);
                }
                else
                {
                    if (HearingSensor.Check(tarC.transform))
                    {
                        HearingTarget.Add(tarC);
                    }
                }
            }
        }

        public void OnFindTarget(SensorTarget target)
        {
            Debug.Log($"感知模块 发现新目标");
        }

        public void OnLostTarget(SensorTarget target)
        {
            Debug.Log($"感知模块 失去目标");
        }
    }
}

//#if UNITY_EDITOR

//namespace Megumin.GameFramework.Sensor
//{
//    using UnityEditor;
//    partial class AISensor
//    {
//        public Color SightRadiusColor;
//        public Color HearingRadiusColor;
//        public bool DrawSphere = false;
//        private void OnDrawGizmosSelected()
//        {
            
//            if (DrawSphere)
//            {
//                //绘制视觉半径
//                Gizmos.color = SightRadiusColor;
//                //Gizmos.DrawSphere(transform.position, SightRadius);
//                var wireColor = Gizmos.color;
//                wireColor.a = 1;
//                Gizmos.color = wireColor;
//                //Gizmos.DrawWireSphere(transform.position, SightRadius);

//                //绘制听觉半径
//                Gizmos.color = HearingRadiusColor;
//                Gizmos.DrawSphere(transform.position, HearingRadius);
//                wireColor = Gizmos.color;
//                wireColor.a = 1;
//                Gizmos.color = wireColor;
//                Gizmos.DrawWireSphere(transform.position, HearingRadius);
//            }

//            Handles.color = SightRadiusColor;
//            Handles.DrawSolidArc(transform.position + Vector3.up, transform.up,
//                transform.forward,
//                SightAngle/2, SightRadius);
//            Handles.DrawSolidArc(transform.position + Vector3.up, transform.up * -1,
//                transform.forward,
//                SightAngle / 2, SightRadius);
//        }
//    }
//}

//#endif

