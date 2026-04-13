using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTransform : CameraFollowStrategy
{
    [SerializeField] private Transform targetTransform;

    public override void FollowTarget()
    {
        if (targetTransform == null)
            return;

        Vector3 followPos = targetTransform.position;
        followPos.z = -10;

        followPos = ConfineCoords(followPos);

        transform.position = followPos;
    }
}
