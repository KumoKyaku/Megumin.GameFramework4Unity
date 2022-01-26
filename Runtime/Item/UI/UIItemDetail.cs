using Megumin.GameFramework.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDetail : MonoBehaviour
{
    public Text Title;
    public Text Prop;
    public Text Des;

    internal void SetData(IMeguminItem data)
    {
        Title.text = data.Name;
        //Prop.text = data.GetProp(0);
        Des.text = data.Description;
    }
}
