using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScalePuzzle : BasePuzzle
{
    [Header("Slots")]
    [SerializeField] private DropSlot slot1;
    [SerializeField] private DropSlot slot2;

    [Header("Correct Combination")]
    [SerializeField] private string correctItem1;
    [SerializeField] private string correctItem2;

    [Header("Visual")]
    [SerializeField] private Image scaleImage;
    [SerializeField] private Sprite state1; 
    [SerializeField] private Sprite state2; 

    [Header("UI")]
    [SerializeField] private Image confirmButtonImage;

    protected override bool CheckSolution()
    {
        if (slot1.currentItem == null || slot2.currentItem == null)
            return false;

        string itemA = slot1.currentItem.itemID;
        string itemB = slot2.currentItem.itemID;

        bool correct =
            (itemA == correctItem1 && itemB == correctItem2) ||
            (itemA == correctItem2 && itemB == correctItem1);

        return correct;
    }

    protected override void CorrectSolution()
    {
        completed = true;

        
        scaleImage.sprite = state2;

       
        if (slot1.currentItem != null)
            slot1.currentItem.gameObject.SetActive(false);

        if (slot2.currentItem != null)
            slot2.currentItem.gameObject.SetActive(false);

        StartCoroutine(WinSequence());
    }

    protected override void IncorrectSolution()
    {
        base.IncorrectSolution(); 

        StartCoroutine(FlashButtonRed());
    }

    IEnumerator FlashButtonRed()
    {
        Color originalColor = confirmButtonImage.color;

        confirmButtonImage.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        confirmButtonImage.color = originalColor;
    }

    IEnumerator WinSequence()
    {
        yield return new WaitForSeconds(3f);

        PuzzleManager.OnPuzzleCompleted();
        DisablePuzzle();
    }
}