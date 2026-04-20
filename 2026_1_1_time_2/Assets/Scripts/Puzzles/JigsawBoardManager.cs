using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawBoardManager : BasePuzzle
{
    [SerializeField] private GameObject[] pieces;

    private int piecesUnlocked = 0;

    public void UnlockPiece(GameObject desiredPiece) 
    {
        foreach (GameObject piece in pieces) 
        {
            if (piece == desiredPiece)
            {
                piece.SetActive(true);
                piecesUnlocked++;
            }
        }
    }

    protected override bool CheckSolution()
    {
        if (piecesUnlocked >= pieces.Length)
            return true;
        return false;
    }

    protected override void CorrectSolution()
    {
        base.CorrectSolution();
        EnablePuzzle();
    }
}
