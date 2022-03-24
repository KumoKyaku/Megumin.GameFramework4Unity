using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public class Sensor : MonoBehaviour
    {
        /// <summary>
        /// 探测器是不是自己进行物理检测
        /// </summary>
        public bool PhysicsTestRadiusSelf = false;
        public LayerMask MaskLayer = -1;

        public TagMask TagMask = new TagMask();

        public virtual List<Collider> PhysicsTest(float mixR)
        {
            var res = Physics.OverlapSphere(transform.position, mixR, MaskLayer);
            List<Collider> colliders = new List<Collider>();
            foreach (var item in res)
            {
                if (TagMask.Check(item))
                {
                    colliders.Add(item);
                }
            }

            return colliders;
        }

        /// <summary>
        /// 更新间隔
        /// </summary>
        [Space]
        [Range(0, 5)]
        public float checkDelta = 0.5f;
        protected float nextCheckStamp;


        static Pref<bool> globalDebugshow;
        /// <summary>
        /// 全局显示开关
        /// </summary>
        public static Pref<bool> GlobalDebugShow
        {
            get
            {
                if (globalDebugshow == null)
                {
                    globalDebugshow = new Pref<bool>(nameof(Sensor), true);
                }
                return globalDebugshow;
            }
        }

        [EditorButton]
        public void SwitchGlobalToggle()
        {
            GlobalDebugShow.Value = !GlobalDebugShow;
        }
    }

    public class SensorType
    {
        public const string Sight = nameof(Sight);
        public const string Hearing = nameof(Hearing);
    }
}
