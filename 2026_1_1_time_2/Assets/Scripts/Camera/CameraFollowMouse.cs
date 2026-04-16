using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : CameraFollowStrategy
{
    [Range(0, 1)]
    [SerializeField] private float deadSpaceWidth;
    [Range(0, 1)]
    [SerializeField] private float deadSpaceHeight;
    [Range(0, 1)]
    [SerializeField] private float deadSpaceRadius;

    [SerializeField] private float followSpeed;

    public override void FollowTarget()
    {
        Vector2 screenMousePos = Input.mousePosition;

        Vector3 followPos = Camera.main.ScreenToWorldPoint(screenMousePos);

        if (InDeadZone(followPos))
        {
            return;
        }

        followPos = ConfineCoords(followPos);
        followPos.z = -10;
        //Vector2 dirVector = followPos - (Vector2)transform.position;
        //if (dirVector.magnitude > 1)
        //    dirVector.Normalize();

        //Vector3 moveVector = dirVector * followSpeed * Time.deltaTime;
        //moveVector.z = 0;

        //transform.position += moveVector;

        transform.position = followPos - followPos.normalized * deadSpaceRadius;
        //transform.position = followPos;
    }

    private bool InDeadZone(Vector2 coords) 
    {
        Vector2 viewportCoords = Camera.main.WorldToViewportPoint(coords);
        Vector2 centeredCoords = viewportCoords - (Vector2.one * 0.5f);

        if (centeredCoords.magnitude < deadSpaceRadius) 
        {
            return true;
        }

        //if (Mathf.Abs(centeredCoords.x) < deadSpaceWidth / 2) 
        //{
        //    if (Mathf.Abs(centeredCoords.y) < deadSpaceHeight / 2)
        //    {
        //        return true;
        //    }
        //}

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 cameraSize = cc.GetCameraSize();

        Color gizmoColor = Color.red;
        gizmoColor.a = 0.5f;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, cameraSize * new Vector3(deadSpaceWidth, deadSpaceHeight, 0));
    }
}
