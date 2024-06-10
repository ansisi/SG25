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
    public itemModel selectedItem; // itemModel -> Item으로 수정

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

        


        if (!isPanelOn)
        {
            CameraLook();
            
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
