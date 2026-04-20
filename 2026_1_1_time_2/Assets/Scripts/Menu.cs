using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] Animator animator;
    [SerializeField] WindowShrinking win;

    private bool started = false;

    public void Start()
    {
        GameObject player = GameManager.instance.GetPlayer();
        player.GetComponent<PlayerController>().SetMovement(false);

        canvas.SetActive(true);
    }

    public void Update()
    {
        if (!started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Start");

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

    public void OnFadeOutEnd() 
    {
        canvas.SetActive(false);

        GameObject player = GameManager.instance.GetPlayer();
        player.GetComponent<PlayerController>().SetMovement(true);

        win.StartShrinking();
    }
}
