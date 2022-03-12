using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework
{
    [Serializable]
    public class LoopConfig
    {
        public bool Enabled = true;
        public Scope Scope;
    }

    [Serializable]
    public class DurationSetting
    {
        [FrameAndTime]
        public int Default = 60;
        [FrameAndTime]
        public int Max = 80;
        [FrameAndTime]
        public int Min = 10;
        public bool Infinite = false;

        [HelpBox("仅支持配置一个Loop")]
        public List<LoopConfig> LoopSetting;

        public int GetValue(int lengthOffset)
        {
            if (Mathf.Clamp(Default, Min, Max) != Default)
            {
                Debug.LogError($"动作长度设置有误");
            }
            var v = Default + lengthOffset;
            v = Mathf.Clamp(v, Min, Max);
            return v;
        }
    }
}
