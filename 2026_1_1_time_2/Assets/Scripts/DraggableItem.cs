using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform startParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public string itemID;

    [HideInInspector]
    public DropSlot currentSlot;

    
    private Vector3 originalPosition;
    private Transform originalParent;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
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
            StartCoroutine(VoltarSuave());
        }
    }

    IEnumerator VoltarSuave()
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