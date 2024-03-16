using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public float armLength = 2.0f; // �÷��̾� ���� ����
    public float objectDistance = 1.0f; // ������Ʈ���� �Ÿ�

    private GameObject selectedObject;
    private Vector3 offset; // ������Ʈ�� �巡���� �� ���콺�� ������Ʈ�� �Ÿ� ����

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

        // ���õ� ������Ʈ�� ���� ��
        if (selectedObject != null)
        {
            // ī�޶��� ���� ��ǥ�� �������� ������Ʈ�� ��ġ ���
            Vector3 objectPosition = Camera.main.transform.position + Camera.main.transform.forward * (armLength + objectDistance);

            // ������Ʈ�� �巡���Ͽ� ���� ��ġ�� �̵�
            selectedObject.transform.position = objectPosition;

            // 'G' Ű�� ������ �κ��丮�� ������Ʈ �߰�
            if (Input.GetKeyDown(KeyCode.G))
            {
                Consumable consumable = selectedObject.GetComponent<Consumable>();
                if (consumable != null && consumable.item != null)
                {
                    // �κ��丮�� ���� á�� ���� ������Ʈ�� �������� ����
                    if (inventory.items.Count < inventory.GetSlots().Length)
                    {
                        inventory.AddItem(consumable.item);
                        Destroy(selectedObject);
                    }
                    else
                    {
                        Debug.Log("�κ��丮�� ���� á���ϴ�.");
                    }
                }
                else
                {
                    Debug.Log("������Ʈ�� ����� �������� �����ϴ�.");
                }
            }
        }
    }

    // ���ڼ� �Ʒ� ������Ʈ ��������
    private GameObject GetObjectUnderMouse()
    {
        // ���콺 ��ġ�� ������� ī�޶󿡼� Ray�� �߻��Ͽ� ���ڼ� �Ʒ��� ������Ʈ�� ã��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        // Raycast�� ����Ͽ� ������Ʈ�� ����
        if (Physics.Raycast(ray, out hitInfo))
        {
            // ����� ������Ʈ�� ��ȯ
            return hitInfo.collider.gameObject;
        }

        // ������Ʈ�� ã�� ���� ��� null�� ��ȯ
        return null;
    }
}