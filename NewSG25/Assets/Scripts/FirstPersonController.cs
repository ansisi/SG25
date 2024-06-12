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
        // 마우스 입력 처리
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 카메라의 위아래 회전 처리
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // 플레이어 바디의 좌우 회전 처리
        transform.Rotate(Vector3.up * mouseX);

        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 왼쪽 버튼이 클릭되었을 때
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Raycast로 쓰레기 오브젝트를 검출하고 Trash 태그를 가지고 있다면 삭제
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
        // 키보드 입력 처리 및 물리 기반 이동
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
