using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController pc;

    private Rigidbody2D rb;

    [Header("Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minTurnMagnitude;
    private Vector2 movement;

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
        rb.velocity = movement.normalized * speed;
    }

    private void GetMovementInput() 
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > minTurnMagnitude) 
        {
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) 
            {
                if (movement.x > 0) 
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
                if (movement.y > 0)
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