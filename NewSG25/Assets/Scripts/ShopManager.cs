using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
[CustomEditor(typeof(ShopManager))]
public class ShopManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShopManager shopManager = (ShopManager)target;

        // Reset Story Models 버튼 생성
        if (GUILayout.Button("Reset ItemModels"))
        {
            shopManager.ResetStoryModels();
        }
    }
}
#endif

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    [SerializeField]
    public List<Shelf> SelfList = new List<Shelf>();
    [SerializeField]
    public itemModel[] itemModels;

#if UNITY_EDITOR
    [ContextMenu("Reset Story Models")]
    public void ResetStoryModels()
    {
        itemModels = Resources.LoadAll<itemModel>(""); // Resources 폴더 아래 모든 StoryModel 불러오기
    }
#endif

    private void Awake()
    {
        Instance = this;
    }

    public void SetShelfItem(int ShelfNumber ,int ItemNumber, int ItemIndex)
    {
        SelfList[ShelfNumber].itemModel[ItemNumber] = FindItemModel(ItemIndex);
    }

    public itemModel GetRamdomModel()
    {        
        itemModel tempModels = itemModels[Random.Range(0, itemModels.Length)];
        return tempModels;
    }

    public itemModel FindItemModel(int index)
    {
        itemModel tempModels = null;
        for (int i = 0; i < itemModels.Length; i++)
        {
            if (itemModels[i].itemIndex == index)
            {
                tempModels = itemModels[i];
                break;

            }
        }

        return tempModels;
    }

}
