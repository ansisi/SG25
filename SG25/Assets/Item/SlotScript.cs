using UnityEngine;

public class SlotScript : MonoBehaviour
{
    public ScriptableObject currentItem; // 아이템 타입 변경 (Item -> ScriptableObject)
    //public Image itemImage;

    public void SetItem(ScriptableObject item) // 아이템 타입 변경 (Item -> ScriptableObject)
    {
        currentItem = item;

        if (item != null)
        {
            // 아이템 이미지 주석 처리 (//itemImage.sprite = item.icon;)
            // 아이템 정보 표시
            string itemName = item.name; // 아이템 이름 접근 (item.itemName -> item.name)
            string itemDescription = item.ToString(); // 아이템 정보 문자열 변환

            // ... (아이템 정보 표시 로직) ...

            // 콘솔창에 아이템 정보 출력
            Debug.Log("아이템 채워짐: " + itemName + " (" + itemDescription + ")");
        }
        else
        {
            //itemImage.sprite = null;
        }
    }
}
