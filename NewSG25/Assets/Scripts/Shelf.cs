using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Shelf))]
public class ShelfEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Shelf shelf = (Shelf)target;

        // Reset Story Models 버튼 생성
        if (GUILayout.Button("Reset Item"))
        {
            shelf.ResetItem();
        }
    }
}
#endif

public class Shelf : MonoBehaviour
{
    [SerializeField]
    public itemModel[] itemModels = new itemModel[4];
    [SerializeField]
    public GameObject[] itemList = new GameObject[4];
    [SerializeField]
    public List<Transform> itemPosList = new List<Transform>();

#if UNITY_EDITOR
    [ContextMenu("Reset Story Models")]
    public void ResetItem()                 // AI 테스트 중에서도 진행 되게
    {
        RamdomitemInit();
        itemObjectInit();
    }
#endif

    public void Start()
    {
        RamdomitemInit();
        itemObjectInit();
    }

    void RamdomitemInit()
    {
        for (int i = 0; i < itemModels.Length; i++)
        {
            itemModels[i] = ShopManager.Instance.GetRamdomModel();

        }
    }

    void itemObjectInit()
    {

        for (int i = 0; i < itemModels.Length; i++)
        {
            itemList[i] = null;
            GameObject temp = Instantiate(itemModels[i].ObjectModel);
            temp.transform.parent = itemPosList[i].transform;
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localRotation = Quaternion.identity;
            ItemData itemDataComponent = temp.AddComponent<ItemData>();
            itemDataComponent.itemIndex = itemModels[i].itemIndex;
            itemDataComponent.ItemName = itemModels[i].ItemName;
            itemDataComponent.cost = itemModels[i].cost;
            itemDataComponent.IconImage = itemModels[i].IconImage;
            itemDataComponent.ObjectModel = itemModels[i].ObjectModel;
            itemList[i] = temp;
        }
    }

    public GameObject RandomGetItem()
    {
        int randomNum = Random.Range(0, itemModels.Length);
        return itemModels[randomNum].ObjectModel;
    }

    public GameObject RandomPickItem()
    {
        int randomNum = Random.Range(0, itemModels.Length);

        if (itemModels[randomNum] != null)
        {
            GameObject temp = itemModels[randomNum].ObjectModel;

            Destroy(itemList[randomNum]);
            itemModels[randomNum] = null;

            return temp;
        }
        else
        {
            return null;
        }
    }

    public int AddItemToShelf(itemModel item, int count)
    {
        int addedCount = 0;
        for (int i = 0; i < itemModels.Length && addedCount < count; i++)
        {
            if (itemModels[i] == null)
            {
                itemModels[i] = item;
                GameObject temp = Instantiate(item.ObjectModel, itemPosList[i].position, itemPosList[i].rotation, itemPosList[i]);
                ItemData itemData = temp.AddComponent<ItemData>();
                itemData.itemIndex = item.itemIndex;
                itemData.ItemName = item.ItemName;
                itemData.cost = item.cost;
                itemData.IconImage = item.IconImage;
                itemData.ObjectModel = item.ObjectModel;
                itemList[i] = temp;
                addedCount++;
            }
        }
        return addedCount;
    }
}


