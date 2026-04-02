using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChimeraPuzzle : BasePuzzle
{
    public DropSlot headSlot;
    public DropSlot bodySlot;
    public DropSlot feetSlot;

    public string correctHead = "Head2";
    public string correctBody = "Body1";
    public string correctFeet = "Feet3";

    public GameObject minigameUI;

    public Button confirmButton;
    private Color originalColor;

    void Start()
    {
        originalColor = confirmButton.image.color;
    }

    public void Confirm()
    {
        if (headSlot.currentItem == null ||
            bodySlot.currentItem == null ||
            feetSlot.currentItem == null)
        {
            Debug.Log("Faltam peças!");
            return;
        }

        if (headSlot.currentItem.itemID == correctHead &&
            bodySlot.currentItem.itemID == correctBody &&
            feetSlot.currentItem.itemID == correctFeet)
        {
            Debug.Log("Acertou!");
            CorrectSolution();
        }
        else
        {
            Debug.Log("Errou!");
            StartCoroutine(FlashRed());
        }
    }

    IEnumerator FlashRed()
    {
        confirmButton.image.color = Color.red;

        yield return new WaitForSecondsRealtime(0.5f);

        confirmButton.image.color = originalColor;
    }
}