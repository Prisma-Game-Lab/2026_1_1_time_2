using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraFollowStrategy : MonoBehaviour
{
    [SerializeField] protected CameraController cc;

    public virtual void FollowTarget() 
    {
        return;
    }

    public Vector3 ConfineCoords(Vector3 followPos) 
    {
        Vector3 confinedPos = followPos;

        Vector2 cameraSize = cc.GetCameraSize();

        Vector2 confinerXBounds = cc.GetConfinerXBound();
        Vector2 confinerYBounds = cc.GetConfinerYBound();

        float confinerXSize = MathF.Abs(confinerXBounds.x) + MathF.Abs(confinerXBounds.y);
        float confinerYSize = MathF.Abs(confinerYBounds.x) + MathF.Abs(confinerYBounds.y);

        if (cameraSize.x > confinerXSize)
        {
            confinedPos.x = confinerXBounds.y + confinerXBounds.x;
        }
        else
        {
            confinedPos.x = MathF.Max(confinedPos.x, confinerXBounds.x + cameraSize.x / 2);
            confinedPos.x = MathF.Min(confinedPos.x, confinerXBounds.y - cameraSize.x / 2);
        }

        if (cameraSize.y > confinerYSize)
        {
            confinedPos.y = confinerYBounds.y + confinerYBounds.x;
        }
        else
        {
            confinedPos.y = MathF.Max(confinedPos.y, confinerYBounds.x + cameraSize.y / 2);
            confinedPos.y = MathF.Min(confinedPos.y, confinerYBounds.y - cameraSize.y / 2);
        }

        return confinedPos;
    }
}
