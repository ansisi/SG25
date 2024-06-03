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
            itemName = "뉴비";
            //아이템 랜덤 생성 코드 추가
        }
    }
}
