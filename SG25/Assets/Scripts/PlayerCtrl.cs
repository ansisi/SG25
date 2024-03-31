using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
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

    private Rigidbody _rigidbody;
    public static PlayerCtrl instance;
    private TimeManager timeManager;

    private bool isCrouching = false;

    private void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        timeManager = FindObjectOfType<TimeManager>();

        //input system 웅크리기
        InputActionMap playerControls = new InputActionMap();
        playerControls.AddAction("Crouch", binding: "<Keyboard>/ctrl");
        playerControls.FindAction("Crouch").performed += ctx => OnCrouchInput(ctx);
        playerControls.Enable();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (timeManager != null && timeManager.isTimeStopped)
        {
            canLook = false;
            curMovementInput = Vector2.zero;
        }
        else
        {
            canLook = true;
        }

        if (canLook)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        if (isCrouching)
        {
            dir *= moveSpeed * 0.5f ;

            //웅크렸을 때 시야 낮춤
            cameraContainer.localPosition = new Vector3(cameraContainer.localPosition.x, 0.5f, cameraContainer.localPosition.z);

            
        }
        else
        {
            dir *= moveSpeed;

            //서있을 때 시야 원래대로
            cameraContainer.localPosition = new Vector3(cameraContainer.localPosition.x, 1.0f, cameraContainer.localPosition.z);
        }
        
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
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
    // 웅크리기 입력을 받는 메서드
    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // 웅크리기 토글
            isCrouching = !isCrouching;
        }
    }
}
