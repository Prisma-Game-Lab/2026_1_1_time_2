using UnityEngine;

public class NoteUI : MonoBehaviour
{
    [SerializeField] private GameObject notePanel;

    public void OpenNote()
    {
        AudioManager.Instance.Play("Papel");
        notePanel.SetActive(true);
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
    }
}