using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public String itemName;
    public int price;

    public void Start()
    {
        if (itemName == "")
        {
            itemName = "����";
            //������ ���� ���� �ڵ� �߰�
        }
    }
}
