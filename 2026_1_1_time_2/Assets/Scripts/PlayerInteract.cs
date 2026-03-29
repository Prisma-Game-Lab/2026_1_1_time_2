using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform interactionCenter;
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
        RaycastHit2D[] hitInfo;
        GameObject interactedObject;

        hitInfo = Physics2D.CircleCastAll(interactionCenter.position, interactionRadius, Vector2.zero, 0, layerMask);

        if (hitInfo.Length == 0)
        {
            print("Hit nothing");
            return;
        }
        else 
        {
            RaycastHit2D closestHit = hitInfo[0];
            float minDistance = Vector2.Distance(closestHit.point, interactionCenter.position);

            foreach (RaycastHit2D hit in hitInfo)
            {
                float currentDistance = Vector2.Distance(hit.point, interactionCenter.position);

                if (currentDistance < minDistance)
                {
                    closestHit = hit;
                    minDistance = currentDistance;
                }
            }

            interactedObject = closestHit.transform.gameObject;
        }

        print(interactedObject.name);

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
        Gizmos.DrawWireSphere(interactionCenter.position, interactionRadius);
    }
}
