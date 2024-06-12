using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public CheckoutSystem checkoutSystem;

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
    public itemModel selectedItem;

    private bool receivedMoney = false;
    private float receivedMoneyTimer = 0f;

    public void ReceiveMoneyFromAIExplicitly(int amount)
    {
        Debug.Log("플레이어가 AI로부터 돈을 받았습니다: " + amount);
        MoveToNextDay();
    }

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shelfShopPanel.ShelfShopPanelOn();
        }

        if (!isPanelOn)
        {
            CameraLook();
        }

        // 돈을 클릭했을 때 즉시 다음 날로 이동
        if (Input.GetMouseButtonDown(0) && receivedMoney)
        {
            MoveToNextDay();
        }
    }


    internal void ReceiveMoneyFromAI(int value)
    {
        receivedMoney = true;
    }

    private void MoveToNextDay()
    {
        FindObjectOfType<TimeManager>().NextDayLogic();
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
        Cursor.visible = true;
        isPanelOn = true;
    }

    public void PanelOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPanelOn = false;
    }
}
