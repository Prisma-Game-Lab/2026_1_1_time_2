using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private GameObject playerInstance;

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject GetPlayer() 
    {
        if (playerInstance == null)
            playerInstance = GameObject.FindWithTag("Player");
        return playerInstance;
    }
}
