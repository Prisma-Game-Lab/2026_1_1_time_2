using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController pc;

    private Rigidbody2D rb;

    [Header("Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minTurnMagnitude;
    [SerializeField] private float minWalkSpeed;

    private Vector2 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetMovementInput();
    }

    private void FixedUpdate()
    {
        switch (pc.currentState) 
        {
            case PlayerState.Idle:
                rb.velocity = moveInput.normalized * speed;
                if (rb.velocity.magnitude > minWalkSpeed)
                    pc.SetCurrentState(PlayerState.Walking);
                break;
            case PlayerState.Walking:
                rb.velocity = moveInput.normalized * speed;
                if (rb.velocity.magnitude <= minWalkSpeed)
                    pc.SetCurrentState(PlayerState.Idle);
                break;
        }
    }

    private void GetMovementInput() 
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (pc.currentState == PlayerState.Blocked)
            return;

        if (moveInput.magnitude > minTurnMagnitude)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                if (moveInput.x > 0)
                {
                    pc.SetFacingDir(Vector2.right);
                }
                else
                {
                    pc.SetFacingDir(Vector2.left);
                }
            }
            else
            {
                if (moveInput.y > 0)
                {
                    pc.SetFacingDir(Vector2.up);
                }
                else
                {
                    pc.SetFacingDir(Vector2.down);
                }
            }
        }
    }
}