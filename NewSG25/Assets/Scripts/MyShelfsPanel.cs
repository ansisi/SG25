using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MyShelfsPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    [SerializeField]
    public itemModel[] itemModels;
    public GameObject itemImagePrefab;
    public GameObject itemListContent;

    public GameObject shelfImage;
    public GameObject shelfListContent;

    public GameObject itemPanel;

    private GameObject draggedItem;
    private Shelf targetShelf;

    public List<Shelf> shelves;

    private Transform originalParent;
    private Vector3 originalPosition;
    private int originalSiblingIndex;
    private Canvas canvas;

    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
        itemModels = Resources.LoadAll<itemModel>("");
        CreateItemList();
        CreateShelfList();
    }

    public void CreateItemList()
    {
        for (int i = 0; i < itemModels.Length; i++)
        {
            // Instantiate itemImagePrefab and set its parent to the current transform (for GridLayoutGroup)
            GameObject itemImage = Instantiate(itemImagePrefab, itemListContent.transform);

            // Get the Image component of the instantiated prefab
            Image imageComponent = itemImage.GetComponent<Image>();

            // Set the sprite from itemModels[i].IconImage
            if (imageComponent != null && itemModels[i].IconImage != null)
            {
                // Create a sprite from the texture
                Sprite iconSprite = Sprite.Create(itemModels[i].IconImage,
                                                  new Rect(0.0f, 0.0f, itemModels[i].IconImage.width, itemModels[i].IconImage.height),
                                                  new Vector2(0.5f, 0.5f));
                imageComponent.sprite = iconSprite;
            }

            AddEventTrigger(itemImage);
        }
    }
    private void AddEventTrigger(GameObject itemImage)
    {
        EventTrigger trigger = itemImage.AddComponent<EventTrigger>();

        // Begin Drag
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener((eventData) => { OnBeginDrag((PointerEventData)eventData); });
        trigger.triggers.Add(beginDragEntry);

        // Drag
        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener((eventData) => { OnDrag((PointerEventData)eventData); });
        trigger.triggers.Add(dragEntry);

        // End Drag
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((eventData) => { OnEndDrag((PointerEventData)eventData); });
        trigger.triggers.Add(endDragEntry);

        // Drop
        EventTrigger.Entry dropEntry = new EventTrigger.Entry();
        dropEntry.eventID = EventTriggerType.Drop;
        dropEntry.callback.AddListener((eventData) => { OnDrop((PointerEventData)eventData); });
        trigger.triggers.Add(dropEntry);
    }
    public void CreateShelfList()
    {
        for (int i = 0; i < shelves.Count; i++)
        {
            GameObject shelfUI = Instantiate(shelfImage, shelfListContent.transform);

            TextMeshProUGUI shelfText = shelfUI.GetComponentInChildren<TextMeshProUGUI>();
            if (shelfText != null)
            {
                shelfText.text = "Shelf " + (i + 1);
            }

            AddShelfEventTrigger(shelfUI, shelves[i]);
        }
    }
    private void AddShelfEventTrigger(GameObject shelfUI, Shelf shelf)
    {
        EventTrigger trigger = shelfUI.AddComponent<EventTrigger>();

        EventTrigger.Entry dropEntry = new EventTrigger.Entry();
        dropEntry.eventID = EventTriggerType.Drop;
        dropEntry.callback.AddListener((eventData) =>
        {
            OnDrop((PointerEventData)eventData);
        });
        trigger.triggers.Add(dropEntry);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            draggedItem = eventData.pointerDrag;
            originalParent = draggedItem.transform.parent;
            originalPosition = draggedItem.transform.localPosition;
            originalSiblingIndex = draggedItem.transform.GetSiblingIndex();
            draggedItem.transform.SetParent(canvas.transform, true);
            Debug.Log("잡았삼");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            draggedItem.transform.position = eventData.position;
            Debug.Log("드래그 중");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = eventData.position;

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);

            targetShelf = null;
            foreach (RaycastResult result in results)
            {
                Shelf shelf = result.gameObject.GetComponent<Shelf>();
                if (shelf != null)
                {
                    targetShelf = shelf;
                    break;
                }
            }

            if (targetShelf != null)
            {
                itemModel droppedItem = draggedItem.GetComponent<itemModel>();
                itemPanel.SetActive(true);

                Debug.Log("진열대에 드랍");
            }
            else
            {
                ResetDraggedItem();
            }

            draggedItem = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem != null && targetShelf == null)
        {
            ResetDraggedItem();
        }
    }

    private void ResetDraggedItem()
    {
        draggedItem.transform.SetParent(originalParent, true);
        draggedItem.transform.localPosition = originalPosition;
        draggedItem.transform.SetSiblingIndex(originalSiblingIndex);
        Debug.Log("원래 위치로 돌아감");
    }
}

