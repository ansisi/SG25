using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public CheckoutSystem checkoutSystem; // ���� �ý���


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
    public Item selectedItem; // itemModel -> Item���� ����

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

        //���� ���� �г� ����
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shelfShopPanel.ShelfShopPanelOn();
        }

        //���콺 ��Ŭ�� ���� �õ�-> �� �ȵ�?;; 
        if (Input.GetMouseButtonDown(1))
        {
            AttemptPurchase();
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
                }
            }
        }
    }

    //���� �õ�
    void AttemptPurchase()
    {
        // ���õ� �������� �ִ��� Ȯ��
        if (selectedItem != null)
        {
            // ���� �ý����� �ִ��� Ȯ��
            if (checkoutSystem != null)
            {
                // ���� �õ�
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
        Cursor.visible = true; // ��� �г��� Ȱ��ȭ�� �� Ŀ�� ���̱�

        isPanelOn = true;
    }

    public void PanelOff()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // ��� �г��� ��Ȱ��ȭ�� �� Ŀ�� �����

        isPanelOn = false;
    }
}
