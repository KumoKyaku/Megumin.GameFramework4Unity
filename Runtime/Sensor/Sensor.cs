using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public class Sensor : MonoBehaviour
    {
        /// <summary>
        /// ̽�����ǲ����Լ�����������
        /// </summary>
        public bool PhysicsTestRadiusSelf = false;
        public LayerMask MaskLayer = -1;
        public List<string> IgnoreTag = new List<string>();

        public virtual List<Collider> PhysicsTest(float mixR)
        {
            var res = Physics.OverlapSphere(transform.position, mixR, MaskLayer);
            List<Collider> colliders = new List<Collider>();
            foreach (var item in res)
            {
                if (CheckTag(item))
                {
                    colliders.Add(item);
                }
            }

            return colliders;
        }

        private bool CheckTag(Collider item)
        {
            foreach (var ignore in IgnoreTag)
            {
                if (item.CompareTag(ignore))
                {
                    return false;
                }
            }

            return true;
        }



        /// <summary>
        /// ���¼��
        /// </summary>
        [Space]
        [Range(0, 5)]
        public float checkDelta = 0.5f;
        protected float nextCheckStamp;


        static Pref<bool> globalDebugshow;
        /// <summary>
        /// ȫ����ʾ����
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
