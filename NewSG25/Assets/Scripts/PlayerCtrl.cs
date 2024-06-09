using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public CheckoutSystem checkoutSystem; // 결제 시스템


    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rb;
    public static PlayerCtrl instance;

    private bool isCrouching = false;

    [SerializeField]
    private GameObject resultPanel;
    private ShelfShopPanel shelfShopPanel;

    public bool isPanelOn = false;
    public Item selectedItem; // itemModel -> Item으로 수정

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        shelfShopPanel = GetComponent<ShelfShopPanel>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraContainer.localPosition = Vector3.zero;
    }


    void Update()
    {

        //선반 상점 패널 열기
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shelfShopPanel.ShelfShopPanelOn();
        }

        //마우스 우클릭 결제 시도-> 왜 안됨?;; 
        if (Input.GetMouseButtonDown(1))
        {
            AttemptPurchase();
        }


        if (!isPanelOn)
        {
            CameraLook();
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
        }
    }

    //결제 시도
    void AttemptPurchase()
    {
        // 선택된 아이템이 있는지 확인
        if (selectedItem != null)
        {
            // 결제 시스템이 있는지 확인
            if (checkoutSystem != null)
            {
                // 결제 시도
                checkoutSystem.selectedItems = new List<Item> { selectedItem };
                checkoutSystem.ProcessPayment();
            }
            else
            {
                Debug.LogWarning("Checkout system not found!");
            }
        }
        else
        {
            Debug.LogWarning("No item selected for purchase!");
        }
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void PanelOn()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // 결과 패널이 활성화될 때 커서 보이기

        isPanelOn = true;
    }

    public void PanelOff()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // 결과 패널이 비활성화될 때 커서 숨기기

        isPanelOn = false;
    }
}
