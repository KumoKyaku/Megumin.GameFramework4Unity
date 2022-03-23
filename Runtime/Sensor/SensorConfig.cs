using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public class SensorConfig : ScriptableObject
    {
        [Range(0,30)]
        public float Radius = 15;
        [Range(0,360)]
        public float Angle = 90;
    }
}
