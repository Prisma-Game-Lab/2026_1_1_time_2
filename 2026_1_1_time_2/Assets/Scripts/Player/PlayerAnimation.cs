using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void OnPlayerStateChange(PlayerState currentState) 
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walking", false);
        animator.SetBool("Blocked", false);

        switch (currentState)
        {
            case PlayerState.Idle:
                animator.SetBool("Idle", true);
                break;
            case PlayerState.Walking:
                animator.SetBool("Walking", true);
                break;
            case PlayerState.Blocked:
                animator.SetBool("Blocked", true);
                break;
        }
    }

    public void OnStep() 
    {
        AudioManager.Instance.Play("Passos");
    }
}
