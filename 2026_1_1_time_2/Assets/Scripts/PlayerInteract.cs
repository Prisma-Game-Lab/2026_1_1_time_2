using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform interactionCenter;
    [SerializeField] private float interactionRadius;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(interactionCenter.position, interactionRadius);

        if (hits.Length == 0)
        {
            Debug.Log("Hit nothing");
            return;
        }

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("Interagiu com: " + hit.name);
                interactable.Interact();
                return;
            }
        }

        Debug.Log("Nenhum objeto interagível encontrado");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(interactionCenter.position, interactionRadius);
    }
}