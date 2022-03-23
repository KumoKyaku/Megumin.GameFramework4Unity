using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public partial class AISensor : MonoBehaviour
    {
        public float SightRadius = 20;
        public float SightAngle = 90;
        public float HearingRadius = 5;

        public List<SensorTarget> SightTarget = new List<SensorTarget>();
        public List<SensorTarget> HearingTarget = new List<SensorTarget>();

        public SensorConfig SightConfig;
        public SensorConfig HearingConfig;


        private void Update()
        {
            var mixR = Mathf.Max(SightRadius, HearingRadius);

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

                var dis = Vector3.Distance(transform.position, item.transform.position);

                if (dis < SightRadius)
                {
                    var deltaAngle = Quaternion.Angle(tarC.transform.rotation, transform.rotation);
                    if (deltaAngle < SightAngle)
                    {
                        //ÔÚÊÓ¾õ·¶Î§ÄÚ
                        SightTarget.Add(tarC);
                    }
                }
                
                if (dis < HearingRadius)
                {
                    HearingTarget.Add(tarC);
                }
            }
        }
    }
}

#if UNITY_EDITOR

namespace Megumin.GameFramework.Sensor
{
    using UnityEditor;
    partial class AISensor
    {
        public Color SightRadiusColor;
        public Color HearingRadiusColor;
        public bool DrawSphere = false;
        private void OnDrawGizmosSelected()
        {
            
            if (DrawSphere)
            {
                //»æÖÆÊÓ¾õ°ë¾¶
                Gizmos.color = SightRadiusColor;
                //Gizmos.DrawSphere(transform.position, SightRadius);
                var wireColor = Gizmos.color;
                wireColor.a = 1;
                Gizmos.color = wireColor;
                //Gizmos.DrawWireSphere(transform.position, SightRadius);

                //»æÖÆÌý¾õ°ë¾¶
                Gizmos.color = HearingRadiusColor;
                Gizmos.DrawSphere(transform.position, HearingRadius);
                wireColor = Gizmos.color;
                wireColor.a = 1;
                Gizmos.color = wireColor;
                Gizmos.DrawWireSphere(transform.position, HearingRadius);
            }

            Handles.color = SightRadiusColor;
            Handles.DrawSolidArc(transform.position + Vector3.up, transform.up,
                transform.forward,
                SightAngle/2, SightRadius);
            Handles.DrawSolidArc(transform.position + Vector3.up, transform.up * -1,
                transform.forward,
                SightAngle / 2, SightRadius);
        }
    }
}

#endif

