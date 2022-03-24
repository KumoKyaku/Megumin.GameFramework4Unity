using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public class Sensor : MonoBehaviour
    {
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
