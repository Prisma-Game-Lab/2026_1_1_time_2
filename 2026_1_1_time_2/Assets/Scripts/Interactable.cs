using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject minigameUI;

    public void Interact()
    {
        minigameUI.SetActive(true);
        Time.timeScale = 0f; // pausa o jogo
    }
}