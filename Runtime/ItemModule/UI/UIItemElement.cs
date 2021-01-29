using Megumin.GameFramework.ItemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItemElement : MonoBehaviour,IPointerEnterHandler
{
    public Image Background;
    public Image Icon;
    public Text Count;

    object Item;
    public void SetData(object value)
    {
        this.Item = value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
}
