using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float mouseSensitivity = 300.0f;
    public float moveSpeed = 5.0f;
    public Transform cameraTransform;
    public Rigidbody playerRigidbody;
    public float xRotation = 0.0f;

    public GameObject shelfShopPanel;
    public GameObject myShelfsPanel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraTransform.localRotation = Quaternion.Euler(10, 0.0f, 0.0f);
    }

    void Update()
    {
        // ���콺 �Է� ó��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ī�޶��� ���Ʒ� ȸ�� ó��
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // �÷��̾� �ٵ��� �¿� ȸ�� ó��
        transform.Rotate(Vector3.up * mouseX);

        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 ���� ��ư�� Ŭ���Ǿ��� ��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Raycast�� ������ ������Ʈ�� �����ϰ� Trash �±׸� ������ �ִٸ� ����
                if (hit.collider.CompareTag("Trash"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            shelfShopPanel.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            myShelfsPanel.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        // Ű���� �Է� ó�� �� ���� ��� �̵�
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        playerRigidbody.MovePosition(playerRigidbody.position + move * Time.fixedDeltaTime);
    }

    public void PanelOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mouseSensitivity = 0f;
    }

    public void PanelOff()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        mouseSensitivity = 300f;
    }
}
