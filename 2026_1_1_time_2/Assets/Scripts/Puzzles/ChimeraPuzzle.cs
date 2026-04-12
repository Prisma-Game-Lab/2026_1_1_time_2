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

    protected override bool CheckSolution()
    {
        if (headSlot.currentItem == null ||
            bodySlot.currentItem == null ||
            feetSlot.currentItem == null)
        {
            return false;
        }

        if (headSlot.currentItem.itemID == correctHead &&
            bodySlot.currentItem.itemID == correctBody &&
            feetSlot.currentItem.itemID == correctFeet)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}