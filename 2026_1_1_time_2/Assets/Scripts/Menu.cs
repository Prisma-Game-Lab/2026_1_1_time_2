using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] WindowShrinking win;
    [SerializeField] GameObject player;
    private bool started = false;
    /*public void OnStartPress()
    {
        animator.SetTrigger("Start");
        win.StartShrinking();
        player.GetComponent<PlayerMovement>().enabled = true;
    }*/

    public void Update()
    {
        if (!started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Start");
                win.StartShrinking();
                player.GetComponent<PlayerMovement>().enabled = true;
                started = true;
            }
        }
    }
    public void OnVolumeSliderChanged(float newValue) // valor 0.0 a 1.0
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(newValue);
        }

    }
}
