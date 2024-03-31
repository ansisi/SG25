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
    public TimeManager timeManager;
    public PlayerCtrl playerCtrl;

    public GameObject PanelInventory;
    bool isPanelActive = false;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        timeManager = FindObjectOfType<TimeManager>();
        playerCtrl = FindObjectOfType<PlayerCtrl>();

        PanelInventory.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPanelActive = !isPanelActive;
            PanelInventory.SetActive(isPanelActive);

            if (timeManager != null)
            {
                timeManager.TimeStop(isPanelActive);
            }
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

        if (selectedObject != null)
        {
            Vector3 objectPosition = Camera.main.transform.position + Camera.main.transform.forward * (armLength + objectDistance);

            selectedObject.transform.position = objectPosition;

            if (Input.GetKeyDown(KeyCode.G))
            {
                Consumable consumable = selectedObject.GetComponent<Consumable>();
                if (consumable != null && consumable.item != null)
                {
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

    private GameObject GetObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.gameObject;
        }

        return null;
    }
}