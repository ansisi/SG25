using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField]
    public itemModel[] itemModel = new itemModel[4];
    [SerializeField]
    public List<GameObject> itemList = new List<GameObject>();
    [SerializeField]
    public List<Transform> itemPosList = new List<Transform>();

    public void Start()
    {
        RamdomitemInit();
        itemObjectInit();
    }

    void RamdomitemInit()
    {
        for (int i = 0; i < itemModel.Length ; i++)
        {
            itemModel[i] = ShopManager.Instance.GetRamdomModel();

        }        
    }

    void itemObjectInit()
    {
        itemList.Clear();
        for (int i = 0; i < itemModel.Length; i++)
        {
            GameObject temp = Instantiate(itemModel[i].ObjectModel);
            temp.transform.parent = itemPosList[i].transform;
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localRotation = Quaternion.identity;
            itemList.Add(temp);
        }
    }

    public GameObject RandomGetItem()
    {
        int randomNum = Random.Range(0, itemModel.Length);
        return itemModel[randomNum].ObjectModel;
    }
}

