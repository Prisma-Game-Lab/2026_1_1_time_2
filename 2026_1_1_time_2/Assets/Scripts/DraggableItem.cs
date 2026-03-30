using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform startParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public string itemID;

    [HideInInspector]
    public DropSlot currentSlot;

    public Transform piecesContainer; 

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;

   
        if (currentSlot != null)
        {
            currentSlot.ClearSlot();
            currentSlot = null;
        }

        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

    
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(piecesContainer);
            transform.localPosition = Vector3.zero;
        }
    }
}