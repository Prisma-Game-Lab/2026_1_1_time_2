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
    [SerializeField] private UnityEvent OnCompletion;

    protected bool completed;

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            DisablePuzzle();
        }
    }

    public virtual void EnablePuzzle() 
    {
        OnEnable.Invoke();
        GameObject player = GameManager.instance.GetPlayer();
        player.GetComponent<PlayerController>().SetMovement(false);
        //Time.timeScale = 0f;
        CameraController.FollowMouse();
    }

    public virtual void DisablePuzzle()
    {
        OnDisable.Invoke();
        GameObject player = GameManager.instance.GetPlayer();
        player.GetComponent<PlayerController>().SetMovement(true);
        //Time.timeScale = 1f;
        CameraController.FollowPlayer();
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
        completed = true;
        OnCompletion.Invoke();
        DisablePuzzle();
    }

    protected virtual void IncorrectSolution() 
    {
        if (shrinkValueOnError != Vector2Int.zero) 
        {
            Vector2Int windowSize = GameWindowManager.GetWindowSize();
            Vector2Int newWindowSize = windowSize - shrinkValueOnError;
            GameWindowManager.SetWindowSize(newWindowSize.x, newWindowSize.y);
        }
    }
}
