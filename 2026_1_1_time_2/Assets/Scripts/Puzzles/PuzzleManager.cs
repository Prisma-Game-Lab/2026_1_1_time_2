using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    [Header("Variables")]
    [SerializeField] private int nPuzzles;

    [Header("Events")]
    [SerializeField] private UnityEvent puzzleCompletedEvent;
    [SerializeField] private UnityEvent allPuzzleCompletedEvent;

    private int nPuzzlesCompleted;

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static void OnPuzzleCompleted() 
    {
        if (instance == null)
        {
            Debug.LogError("No PuzzleManager on scene");
            return;
        }

        instance.nPuzzlesCompleted++;

        if (instance.nPuzzlesCompleted >= instance.nPuzzles) 
        {
            instance.allPuzzleCompletedEvent.Invoke();
            return;
        }

        instance.puzzleCompletedEvent.Invoke();
    } 
}
