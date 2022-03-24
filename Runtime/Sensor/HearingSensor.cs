using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Megumin.GameFramework.Sensor
{
    public partial class HearingSensor : Sensor
    {
        [ReadOnlyInInspector]
        public string Type = SensorType.Hearing;

        [Range(0, 30)]
        public float Radius = 7.5f;

        /// <summary>
        /// 更新间隔
        /// </summary>
        [Space]
        [Range(0, 5)]
        public float checkDelta = 0.5f;
        /// <summary>
        /// 每次增加目标听觉值
        /// </summary>
        [Range(0, 50)]  
        public int AddValueInRange = 10;
        /// <summary>
        ///每次减少目标听觉值
        /// </summary>
        [Range(0, 50)]
        public int RemoveValueOutRange = 10;
        /// <summary>
        /// 触发被听见的阈值
        /// </summary>
        [Range(0, 100)]
        public int TriggerValue = 50;
        /// <summary>
        /// 最大累计听觉值，这个值是为了脱离范围能很快消退被感知
        /// </summary>
        [Range(0, 100)]
        public int MaxSumValue = 100;

        private float nextCheckStamp;
        public void Update()
        {
            if (Time.time < nextCheckStamp)
            {
                return;
            }
            nextCheckStamp = Time.time + checkDelta;

            using var _handle = ListPool<MonoBehaviour>.Rent(out var list);
            list.AddRange(hearingdelta.Keys);

            //每次更新减少10点听觉值，小于0就移除
            foreach (var item in list)
            {
                var v = hearingdelta[item];
                
                var dis = Vector3.Distance(transform.position, item.transform.position);
                if (dis < Radius)
                {
                    //每次在范围内就增加听觉值
                    v += AddValueInRange * checkDelta;
                    v = Mathf.Min(v, MaxSumValue);
                }
                else
                {
                    v -= RemoveValueOutRange * checkDelta;
                }

                hearingdelta[item] = v;

                if (v < 0)
                {
                    hearingdelta.Remove(item);
                }
            }
        }

        Dictionary<MonoBehaviour, float> hearingdelta = new Dictionary<MonoBehaviour, float>();
        public bool Check(MonoBehaviour target)
        {
            if (!enabled)
            {
                return false;
            }

            var current = 0f;
            if (hearingdelta.TryGetValue(target, out var delta))
            {
                current = delta;
            }
            else
            {
                hearingdelta[target] = current;
            }

            //在视听觉围内
            return current >= TriggerValue;
        }

        [Header("Debug")]
        [Space]
        public bool DebugSolid = false;
        public Color DebugColor = Color.green;
        [Range(0, 20)]
        public float DebugLineThickness = 5;
    }
}


#if UNITY_EDITOR

namespace Megumin.GameFramework.Sensor
{
    using UnityEditor;
    partial class HearingSensor
    {
        private void OnDrawGizmosSelected()
        {
            //绘制听觉半径
            Gizmos.color = DebugColor;
            if (DebugSolid)
            {
                Gizmos.DrawSphere(transform.position, Radius);
            }
           
            var wireColor = Gizmos.color;
            wireColor.a = 1;
            Gizmos.color = wireColor;
            Gizmos.DrawWireSphere(transform.position, Radius);

            Handles.color = wireColor;
            foreach (var item in hearingdelta)
            {
                Handles.Label(item.Key.transform.position + Vector3.up, item.Value.ToString(),
                    GUI.skin.textField);
            }
        }
    }
}

#endif

