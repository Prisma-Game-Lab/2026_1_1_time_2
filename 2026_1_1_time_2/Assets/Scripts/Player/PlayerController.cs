using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private UnityEvent<PlayerState> OnPlayerStateChange;

    public PlayerState currentState { get; private set; } = PlayerState.Idle;

    private Vector2 facingVector = Vector2.down;

    public void AllowMovement() 
    {
        SetCurrentState(PlayerState.Idle);
    }

    public void BlockMovement() 
    {
        SetCurrentState(PlayerState.Blocked);
    }

    public Vector2 GetFacingDir() 
    {
        return facingVector;
    }

    public void SetFacingDir(Vector2 facingVector) 
    {
        this.facingVector = facingVector;
    }

    public void SetCurrentState(PlayerState playerState) 
    {
        if (currentState == playerState)
            return;
        currentState = playerState;
        OnPlayerStateChange.Invoke(currentState);
    }
}

public enum PlayerState 
{
    Idle, Walking, Blocked
}