using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleStarter : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent OnInteraction;

    public void Interact() 
    {
        print("Started Puzzle");
        OnInteraction.Invoke();
    }
}
