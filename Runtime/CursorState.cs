using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// 全局鼠标状态控制器
    /// </summary>
    public static class CursorState
    {
        public static readonly AnyTrueControl Visible = new(onValueChangedKV: OnVisibleChange);
        public static readonly AnyTrueControl Unlock = new(onValueChangedKV: OnUnlockChange);

        static void OnVisibleChange((object, bool) newValue, (object, bool) oldValue)
        {
            Cursor.visible = newValue.Item2;
            Debug.Log($"OnVisibleChange newValue:{newValue}    ----    oldValue:{oldValue}");
        }

        static void OnUnlockChange((object, bool) newValue, (object, bool) oldValue)
        {
            Cursor.lockState = newValue.Item2 ? CursorLockMode.None : CursorLockMode.Locked;
            Debug.Log($"OnUnlockChange newValue:{newValue}    ----    oldValue:{oldValue}");
        }

        public static void Refresh(bool forceRaiseEvent = false)
        {
            Visible.Refresh(forceRaiseEvent);
            Unlock.Refresh(forceRaiseEvent);
        }
    }
}



