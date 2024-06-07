using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Shelf))]
public class ShelfEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Shelf shelf = (Shelf)target;

        // Reset Story Models ��ư ����
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
    public void ResetItem()                 // AI �׽�Ʈ �߿����� ���� �ǰ�
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
        for (int i = 0; i < itemModels.Length ; i++)
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

        if(itemModels[randomNum] != null)
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
}
