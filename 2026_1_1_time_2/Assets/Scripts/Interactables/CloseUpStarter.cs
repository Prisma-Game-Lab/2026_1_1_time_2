using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpStarter : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject closeUp;

    public void Interact()
    {
        CloseUpManager.instance.Open(closeUp.name);
    }
}
