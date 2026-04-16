using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpManager : MonoBehaviour
{
    [SerializeField] private CloseUp[] closeUps;

    public void Open(string name) 
    {
        
    }

    public void Close()
    {
        
    }
}

[Serializable]
public class CloseUp 
{
    [SerializeField] private string name;
    [SerializeField] private GameObject closeUpObject;
}
