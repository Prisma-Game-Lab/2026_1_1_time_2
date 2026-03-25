using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindowDeathSequence : MonoBehaviour
{
    [SerializeField] private UnityEvent OnDeathSequenceStart;

    private bool editorMode;

    private void Start()
    {
        #if UNITY_EDITOR
        editorMode = true;
        #endif
    }

    public void IniciateDeathSequence() 
    {
        OnDeathSequenceStart.Invoke();

        if (editorMode) 
        {
            print("Morreu");
        }
        else
        {
            Application.Quit();
        }
    }
}
