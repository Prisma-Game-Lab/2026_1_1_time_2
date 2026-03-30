using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string slotType; // "Head", "Body", "Feet"
    public DraggableItem currentItem;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();

        // 🚫 slot já ocupado → não deixa colocar outra
        if (currentItem != null)
        {
            Debug.Log("Slot já ocupado!");
            return;
        }

        if (item != null && item.itemID.StartsWith(slotType))
        {
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;

            currentItem = item;
            item.currentSlot = this; // 👈 agora isso funciona corretamente
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
    }
}