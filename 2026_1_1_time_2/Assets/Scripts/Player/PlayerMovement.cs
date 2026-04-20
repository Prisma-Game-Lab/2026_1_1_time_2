using UnityEngine;
using UnityEngine.TerrainTools;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController pc;

    private Rigidbody2D rb;

    [Header("Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minTurnMagnitude;
    private Vector2 movement;
    private bool passos = false;

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
        if (pc.canMove)
        {
            rb.velocity = movement.normalized * speed;
            if (movement.normalized.magnitude == 0f)
            {
                AudioManager.Instance.Stop("Passos");
                passos = false;
            }
            else
            {
                if (!passos)
                    AudioManager.Instance.Play("Passos");
                passos = true;
            }
        }
    }

    private void GetMovementInput() 
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (!pc.canMove)
            return;

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