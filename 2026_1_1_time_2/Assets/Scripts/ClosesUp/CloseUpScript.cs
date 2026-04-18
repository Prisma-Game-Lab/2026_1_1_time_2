using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseUpScript : MonoBehaviour
{
    public void Close() 
    {
        CloseUpManager.instance.Close();
    }

    public void OpenSubcloseUp(GameObject subCloseUp) 
    {
        CloseUpManager.instance.Open(subCloseUp.name);
    }
}
