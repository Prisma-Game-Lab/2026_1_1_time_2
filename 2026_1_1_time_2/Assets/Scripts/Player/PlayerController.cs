using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 facingVector = Vector2.down;

    public Vector2 GetFacingDir() 
    {
        return facingVector;
    }

    public void SetFacingDir(Vector2 facingVector) 
    {
        this.facingVector = facingVector;
    }
}
