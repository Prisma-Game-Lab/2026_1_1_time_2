using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePuzzle : MonoBehaviour
{
    [Header("Base Puzzle Variables")]
    [SerializeField] private Vector2Int shrinkValueOnError;

    [Header("Base Puzzle Events")]
    [SerializeField] private UnityEvent OnEnable;
    [SerializeField] private UnityEvent OnDisable;

    [SerializeField] private PlayerMovement player;

    protected bool completed;

    public virtual void EnablePuzzle() 
    {
        OnEnable.Invoke();
        player.enabled = false;
        //Time.timeScale = 0f;
        //CameraController.SetFollowTarget(mouseObject);
    }

    public virtual void DisablePuzzle()
    {
        OnDisable.Invoke();
        player.enabled = true;
        //Time.timeScale = 1f;
        //CameraController.SetFollowTarget(playerObject);
    }

    public virtual void ValidateSolution() 
    {
        if (completed) return;

        if (CheckSolution()) 
        {
            CorrectSolution();
        }
        else 
        {
            IncorrectSolution();
        }
    }

    protected virtual bool CheckSolution() 
    {
        return false;
    }

    protected virtual void CorrectSolution() 
    {
        print("Correct Solution");
        completed = true;
        PuzzleManager.OnPuzzleCompleted();
        DisablePuzzle();
    }

    protected virtual void IncorrectSolution() 
    {
        print("Incorrect Solution");

        if (shrinkValueOnError != Vector2Int.zero) 
        {
            Vector2Int windowSize = GameWindowManager.GetWindowSize();
            Vector2Int newWindowSize = windowSize - shrinkValueOnError;
            GameWindowManager.SetWindowSize(newWindowSize.x, newWindowSize.y);
        }
    }
}
