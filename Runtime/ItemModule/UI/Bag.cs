using Megumin.GameFramework.ItemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public UIItemElement Template;
    public Transform Content;
    public UIItemDetail IItemDetail;

    private void Awake()
    {
        Template.gameObject.SetActive(false);
    }

    public void Open(TestItemModule module)
    {
        foreach (Transform item in Content)
        {
            if (item != Template.transform)
            {
                Destroy(item.gameObject);
            }
        }

        foreach (var item in module.MyItems)
        {
            var ele = Instantiate(Template, Template.transform.parent);
            ele.gameObject.SetActive(true);
            ele.SetData(item.Value);
        }
    }
}
