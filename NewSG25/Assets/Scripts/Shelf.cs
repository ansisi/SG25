using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public GameObject[] items;

    public GameObject RandomGetItem()
    {
        int randomNum = Random.Range(0, items.Length);
        return items[randomNum];
    }

}


//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class ItemData
//{
//    public string[] itemNames = { "Barleysnack", "CrayFishCracker", "OhCarrot", "SweetPotatoChips" };
//    public string itemName;
//    public int itemPrice;
//    public int itemIndex;
//    public List<GameObject> items= new List<GameObject>();

//    public void ItemName()
//    {
//        if (itemIndex >= 0 && itemIndex < itemNames.Length)
//        {
//            itemName = itemNames[itemIndex];
//        }
//    }
//}

//public class Shelf : MonoBehaviour
//{
//    public int shelfIndex1;
//    public int shelfIndex2;

//    public Transform waypoint1;
//    public Transform waypoint2;

//    public GameObject[] items1;
//    public GameObject[] items2;
//    public GameObject[] items;

//    void Start()
//    {

//    }

//    public GameObject RandomGetItem()
//    {
//        int randomNum = Random.Range(0, items.Length);
//        return items[randomNum];
//    }

//    public GameObject AddItem1()
//    {
//        ItemData itemData = new ItemData();
//        itemData.itemIndex = shelfIndex1;
//        itemData.ItemName();

//        for (int i = 0; i < items1.Length; i++)
//        {
//            itemData.items.Add(items1[i]);
//        }

//        int itemCount = 1;
//        items1[itemCount].gameObject.SetActive(false);

//        Debug.Log("번호 " + shelfIndex1 + "개수 " + itemData.items.Count + "들어있는 아이템 이름" + itemData.itemName);
//        return items1[itemCount];

//    }


//    public GameObject AddItem2()
//    {
//        ItemData itemData = new ItemData();
//        itemData.itemIndex = shelfIndex2;
//        itemData.ItemName();

//        for (int i = 0; i < items2.Length; i++)
//        {
//            itemData.items.Add(items2[i]);
//        }

//        int itemCount = 1;
//        items2[itemCount].gameObject.SetActive(false);

//        Debug.Log("번호 " + shelfIndex2 + "개수 " + itemData.items.Count + "들어있는 아이템 이름" + itemData.itemName);
//        return items2[itemCount];
//    }    
//}
