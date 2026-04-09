using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : CameraFollowStrategy
{
    [SerializeField] private float deadSpaceWidth;
    [SerializeField] private float deadSpaceHeight;

    public override void FollowTarget()
    {
        if (targetTransform == null)
            return;

        Vector2 mousePos = Input.mousePosition;


        Vector3 followPos = targetTransform.position;
        followPos.z = -10;

        followPos = ConfineCoords(followPos);

        transform.position = followPos;
    }

    private void InDeadZone(Vector2 coords) 
    {
        Vector2 cameraSize = cc.GetCameraSize();

        Vector2 deadZoneXBounds;
        deadZoneXBounds.x = (cameraSize.x / 2) - deadSpaceWidth;
        deadZoneXBounds.y = (cameraSize.x / 2) + deadSpaceWidth;
        
        Vector2 deadZoneYBounds;
        deadZoneYBounds.x = (cameraSize.y / 2) - deadSpaceHeight;
        deadZoneYBounds.y = (cameraSize.y / 2) + deadSpaceHeight;


    }
}
