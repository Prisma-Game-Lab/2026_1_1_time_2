using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject closeUpButton;

    [Header("Config")]
    [SerializeField] private string subCloseUp;

    private void Start()
    {
        if (subCloseUp != "")
            closeUpButton.SetActive(true);
    }

    public void Close() 
    {
        CloseUpManager.instance.Close();
    }

    public void OpenSubcloseUp() 
    {
        CloseUpManager.instance.Open(subCloseUp);
    }
}
