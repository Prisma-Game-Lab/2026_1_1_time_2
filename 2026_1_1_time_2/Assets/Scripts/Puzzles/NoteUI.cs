using UnityEngine;

public class NoteUI : MonoBehaviour
{
    [SerializeField] private GameObject notePanel;

    public void OpenNote()
    {
        notePanel.SetActive(true);
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
    }
}