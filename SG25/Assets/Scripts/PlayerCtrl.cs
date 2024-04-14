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

    private Rigidbody rb;
    public static PlayerCtrl instance;
    private TimeManager timeManager;

    private bool isCrouching = false;

    public GameObject orderPanel;
    [SerializeField]
    private GameObject resultPanel;

    public bool isPanelOn = false;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //timeManager = FindObjectOfType<TimeManager>();

        //input system ��ũ����
        InputActionMap playerControls = new InputActionMap();
        playerControls.AddAction("Crouch", binding: "<Keyboard>/ctrl");
        playerControls.FindAction("Crouch").performed += ctx => OnCrouchInput(ctx);
        playerControls.Enable();
    }

    void FixedUpdate()
    {
        Move();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            orderPanel.SetActive(true);
        }

        if (!isPanelOn)
        {
            CameraLook();
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

                    // Raycast�� ���� ���� ������Ʈ�� �����ϰ� BrokenBottle �±׸� ������ �ִٸ� ����
                    if (hit.collider.CompareTag("BrokenBottle"))
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }
    }


    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        if (isCrouching)
        {
            dir *= moveSpeed * 0.5f ;

            //��ũ���� �� �þ� ����
            cameraContainer.localPosition = new Vector3(cameraContainer.localPosition.x, 0.5f, cameraContainer.localPosition.z);

            
        }
        else
        {
            dir *= moveSpeed;

            //������ �� �þ� �������
            cameraContainer.localPosition = new Vector3(cameraContainer.localPosition.x, 1.0f, cameraContainer.localPosition.z);
        }
        
        dir.y = rb.velocity.y;

        rb.velocity = dir;
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
    // ��ũ���� �Է��� �޴� �޼���
    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // ��ũ���� ���
            isCrouching = !isCrouching;
        }
    }

    public void PanelOn()
    {
        resultPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // ��� �г��� Ȱ��ȭ�� �� Ŀ�� ���̱�

        isPanelOn = true;
    }

    public void PanelOff()
    {
        resultPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // ��� �г��� ��Ȱ��ȭ�� �� Ŀ�� �����

        isPanelOn = false;
    }
}
