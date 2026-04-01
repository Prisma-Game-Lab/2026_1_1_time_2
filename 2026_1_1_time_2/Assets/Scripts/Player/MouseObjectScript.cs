using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObjectScript : MonoBehaviour
{
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = mousePos;
    }
}
