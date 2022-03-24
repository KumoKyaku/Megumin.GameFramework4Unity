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
