using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraFollowStrategy : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    public virtual void FollowTarget() 
    {
        if (targetTransform == null)
            return;

        Vector3 followPos = targetTransform.position;
        followPos.z = -10;

        transform.position = followPos;
    }
}
