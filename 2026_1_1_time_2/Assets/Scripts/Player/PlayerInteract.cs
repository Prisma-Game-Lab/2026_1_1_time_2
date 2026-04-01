using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController pc;

    [Header("Variables")]
    [SerializeField] private float interactionDistance;
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask layerMask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Vector2 facingDir = pc.GetFacingDir();
        Vector2 interactPos = (Vector2)transform.position + interactionDistance * facingDir;

        Collider2D[] hits = Physics2D.OverlapCircleAll(interactPos, interactionRadius, layerMask);

        if (hits.Length == 0)
        {
            Debug.Log("Hit nothing");
            return;
        }

        Collider2D closestHit = hits[0];
        float minDistance = Vector2.Distance(closestHit.transform.position, interactPos);

        foreach (Collider2D hit in hits)
        {
            float currentDistance = Vector2.Distance(hit.transform.position, interactPos);

            if (currentDistance < minDistance)
            {
                closestHit = hit;
                minDistance = currentDistance;
            }
        }

        GameObject interactedObject = closestHit.gameObject;
        IInteractable interactableScript = interactedObject.GetComponent<IInteractable>();

        if (interactableScript == null)
        {
            print("No interactable script in object");
            return;
        }

        interactableScript.Interact();
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 facingDir = pc.GetFacingDir();
        Vector2 interactPos = (Vector2)transform.position + interactionDistance * facingDir;

        Gizmos.DrawWireSphere(interactPos, interactionRadius);
    }
}