using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public string itemID;

    [HideInInspector]
    public DropSlot currentSlot;

    private Vector3 originalPosition;
    private Transform originalParent;

    private Vector3 dragOffset;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentSlot != null)
        {
            currentSlot.ClearSlot();
            currentSlot = null;
        }

        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;

        dragOffset = transform.position - GetMouseWorldPos();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPos() + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        
        if (transform.parent == canvas.transform)
        {
            StartCoroutine(VoltarSuave());
        }
    }

    private Vector3 GetMouseWorldPos() 
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        return worldMousePos;
    }

    private IEnumerator VoltarSuave()
    {
        float tempo = 0f;
        float duracao = 0.2f;

        Vector3 inicio = transform.position;

        while (tempo < duracao)
        {
            transform.position = Vector3.Lerp(inicio, originalPosition, tempo / duracao);
            tempo += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.SetParent(originalParent);
    }
}