using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpManager : MonoBehaviour
{
    [SerializeField] private CloseUp[] closeUps;

    private Dictionary<string, CloseUp> closeUpDict;
    private Stack<CloseUp> closeUpStack = new Stack<CloseUp>();
    private CloseUp currentCloseUp;

    private void Start()
    {
        closeUpDict = new Dictionary<string, CloseUp>();
        foreach (CloseUp closeUp in closeUps) 
        {
            closeUpDict.Add(closeUp.name, closeUp);
        }
    }

    public void Open(string name) 
    {
        if (closeUpDict.TryGetValue(name, out CloseUp value))
        {
            OpenSimple(value);
        }
        else
        {
            print($"Wasn't Able to find closeUp {name}");
        }
    }

    public void Close()
    {
        CloseSimple();
        if (closeUpStack.Count > 0)
        {
            CloseUp nextCloseUp = closeUpStack.Pop();
            OpenSimple(nextCloseUp);
        }
    }

    private void OpenSimple(CloseUp closeUp) 
    {
        if (currentCloseUp != null) 
        {
            closeUpStack.Push(currentCloseUp);
            CloseSimple();
        }

        currentCloseUp = closeUp;
        currentCloseUp.closeUpObject.SetActive(true);
    }

    private void CloseSimple() 
    {
        if (currentCloseUp == null)
        {
            return;
        }

        currentCloseUp.closeUpObject.SetActive(false);
        currentCloseUp = null;

        return;
    }
}

[Serializable]
public class CloseUp 
{
    public string name;
    public GameObject closeUpObject;
}
