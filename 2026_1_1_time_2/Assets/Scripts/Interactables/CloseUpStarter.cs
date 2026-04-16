using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpStarter : MonoBehaviour, IInteractable
{
    [SerializeField] private string closeUpName;

    public void Interact()
    {
        print("Open closeUp");
        CloseUpManager.instance.Open(closeUpName);
    }
}
