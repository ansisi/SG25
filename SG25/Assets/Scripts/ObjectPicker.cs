using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public float armLength = 2.0f; // 플레이어 팔의 길이
    public float objectDistance = 1.0f; // 오브젝트와의 거리

    private GameObject selectedObject;
    private Vector3 offset; // 오브젝트를 드래그할 때 마우스와 오브젝트의 거리 차이

    private Inventory inventory;

    public GameObject PanelInventory;
    bool isPanelActive = false;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        PanelInventory.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPanelActive = !isPanelActive;
            PanelInventory.SetActive(isPanelActive);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject == null)
            {
                GameObject pickedObject = GetObjectUnderMouse();

                if (pickedObject != null && pickedObject.CompareTag("Item"))
                {
                    selectedObject = pickedObject;
                    offset = selectedObject.transform.position - Camera.main.transform.position;
                }
            }
            else
            {
                selectedObject = null;
            }
        }

        // 선택된 오브젝트가 있을 때
        if (selectedObject != null)
        {
            // 카메라의 월드 좌표를 기준으로 오브젝트의 위치 계산
            Vector3 objectPosition = Camera.main.transform.position + Camera.main.transform.forward * (armLength + objectDistance);

            // 오브젝트를 드래그하여 계산된 위치로 이동
            selectedObject.transform.position = objectPosition;

            // 'G' 키가 눌리면 인벤토리에 오브젝트 추가
            if (Input.GetKeyDown(KeyCode.G))
            {
                Consumable consumable = selectedObject.GetComponent<Consumable>();
                if (consumable != null && consumable.item != null)
                {
                    // 인벤토리가 가득 찼을 때는 오브젝트를 삭제하지 않음
                    if (inventory.items.Count < inventory.GetSlots().Length)
                    {
                        inventory.AddItem(consumable.item);
                        Destroy(selectedObject);
                    }
                    else
                    {
                        Debug.Log("인벤토리가 가득 찼습니다.");
                    }
                }
                else
                {
                    Debug.Log("오브젝트에 연결된 아이템이 없습니다.");
                }
            }
        }
    }

    // 십자선 아래 오브젝트 가져오기
    private GameObject GetObjectUnderMouse()
    {
        // 마우스 위치를 기반으로 카메라에서 Ray를 발사하여 십자선 아래의 오브젝트를 찾음
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        // Raycast를 사용하여 오브젝트를 검출
        if (Physics.Raycast(ray, out hitInfo))
        {
            // 검출된 오브젝트를 반환
            return hitInfo.collider.gameObject;
        }

        // 오브젝트를 찾지 못한 경우 null을 반환
        return null;
    }
}