using UnityEngine;

public class CubeClick : MonoBehaviour
{
    public GameObject myItem; // 생성할 프리팹 오브젝트

    public bool hasItem = false; // 이 슬롯이 아이템을 가지고 있는가

    public void GetItem(GameObject target)
    {
        myItem = target;
        // 큐브의 위치를 가져옵니다.
        Vector3 cubePosition = transform.position;

        // 생성된 오브젝트의 위치를 큐브 위치로 설정합니다.
        myItem.transform.position = cubePosition;

        // 생성된 오브젝트의 이름을 변경합니다.
        myItem.name = "NewPrefabObject";

        // 콘솔에 생성된 오브젝트의 이름을 출력합니다.
        Debug.Log("Object instantiated: " + myItem.name);

        // 슬롯에 아이템이 있다고 바꿉니다.
        hasItem = true;
    }

    public GameObject GiveItem()
    {
        if(myItem != null)
        {
            hasItem = false;
            // 아이템의 콜라이더를 비활성화 합니다
            myItem.GetComponent<Collider>().enabled = hasItem;

            // 이 슬롯에 아이템을 다시 받을 수 있게 활성화 합니다.
            this.GetComponent<Collider>().enabled = !hasItem;

            return myItem;
        }
        else
        {
            return null;
        }
    }
}
