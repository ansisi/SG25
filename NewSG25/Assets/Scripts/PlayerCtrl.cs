using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    private bool isCrouching = false;

    [SerializeField]
    private GameObject resultPanel;

    public bool isPanelOn = false;

    private HealthManager healthManager; // HealthManager 인스턴스

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraContainer.localPosition = Vector3.zero;

        // HealthManager 인스턴스 가져오기
        healthManager = HealthManager.Instance;

        if (healthManager == null)
        {
            Debug.LogError("HealthManager 인스턴스를 찾을 수 없습니다.");
        }

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadMiniGameScene();
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

                        // 쓰레기를 치웠으므로 체력을 5 증가시킴
                        if (healthManager != null)
                        {
                            healthManager.IncreaseHealth(5);
                        }
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
            dir *= moveSpeed * 0.5f;

            //웅크렸을 때 시야 낮춤
            cameraContainer.localPosition = new Vector3(cameraContainer.localPosition.x, 0.5f, cameraContainer.localPosition.z);


        }
        else
        {
            dir *= moveSpeed;

            //서있을 때 시야 원래대로
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
    // 웅크리기 입력을 받는 메서드
    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // 웅크리기 토글
            isCrouching = !isCrouching;
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
    private void LoadMiniGameScene()
    {
        SceneManager.LoadScene("DrinkMiniGameScene"); // "MiniGameScene"을 미니게임 씬의 이름으로 변경
    }


}
