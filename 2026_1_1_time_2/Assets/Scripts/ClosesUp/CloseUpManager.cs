using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpManager : MonoBehaviour
{
    public static CloseUpManager instance;

    [SerializeField] private GameObject closeUpHolder;

    private Dictionary<string, GameObject> closeUpDict;
    private Stack<GameObject> closeUpStack = new Stack<GameObject>();
    private GameObject currentCloseUp;

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        closeUpDict = new Dictionary<string, GameObject>();

        for (int i = 0; i < closeUpHolder.transform.childCount; i++)
        {
            Transform closeUpTransform = closeUpHolder.transform.GetChild(i);
            GameObject closeUpObj = closeUpTransform.gameObject;

            closeUpDict.Add(closeUpObj.name, closeUpObj);
        }
    }

    public void Open(string name) 
    {
        if (closeUpDict.TryGetValue(name, out GameObject value))
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
            GameObject nextCloseUp = closeUpStack.Pop();
            OpenSimple(nextCloseUp);
        }
    }

    private void OpenSimple(GameObject closeUp) 
    {
        if (currentCloseUp != null) 
        {
            closeUpStack.Push(currentCloseUp);
            CloseSimple();
        }

        currentCloseUp = closeUp;
        currentCloseUp.SetActive(true);
    }

    private void CloseSimple() 
    {
        if (currentCloseUp == null)
        {
            return;
        }

        currentCloseUp.SetActive(false);
        currentCloseUp = null;

        return;
    }
}
