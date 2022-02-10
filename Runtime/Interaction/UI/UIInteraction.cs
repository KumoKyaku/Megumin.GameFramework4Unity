using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Megumin.GameFramework.Interaction
{
    public class UIInteraction : MonoBehaviour
    {
        public Image InteractionIcon = default;

        public void SetIcon(Sprite icon)
        {
            InteractionIcon.sprite = icon;
        }
    }
}
