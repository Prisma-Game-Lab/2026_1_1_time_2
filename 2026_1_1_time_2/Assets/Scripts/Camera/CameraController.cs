using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    [SerializeField] private static CameraController instance;

    [SerializeField] private PixelPerfectCamera ppCamera;
    [SerializeField] private Transform targetTransform;
    //[SerializeField] private CinemachineConfiner2D confiner2D;

    [SerializeField] private float minWindowUpdateDistance;
    [SerializeField] private float minWindowUpdateDifference;

    private Vector2 confinerXBounds;
    private Vector2 confinerYBounds;
    float pixelPerUnit = 100;

    private Vector2 previousPos;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);    
        }

        instance = this;
    }

    private void Start()
    {
        CalculatePixelPerUnit();

        previousPos = transform.position;

        Vector2Int windowSize = GameWindowManager.GetWindowSize();

        SetCameraSize(windowSize.x, windowSize.y);
        CalculateConfiner();
    }

    private void OnEnable()
    {
        GameWindowManager.instance.OnWindowSizeChange.AddListener(SetCameraSize);
    }

    private void OnDisable()
    {
        GameWindowManager.instance.OnWindowSizeChange.RemoveListener(SetCameraSize);
    }

    private void Update()
    {
        FollowTarget();

        if (Vector2.Distance(transform.position, previousPos) > minWindowUpdateDistance) 
        {
            previousPos = transform.position;
            UpdateWindowPos();
        }
    }

    private void CalculatePixelPerUnit() 
    {
        Vector2 screenPos1 = Camera.main.WorldToScreenPoint(Vector2.zero);
        Vector2 screenPos2 = Camera.main.WorldToScreenPoint(Vector2.one);

        Vector2 diff = screenPos2 - screenPos1;

        pixelPerUnit = diff.x;
    }

    private void CalculateConfiner() 
    {
        Vector3 confinerScale = (Vector2)GameWindowManager.GetScreenCenterPos() / pixelPerUnit;
        confinerXBounds = new Vector2(-confinerScale.x, confinerScale.x);
        confinerYBounds = new Vector2(-confinerScale.y, confinerScale.y);
        confinerScale.z = 1;
    }

    private void FollowTarget() 
    {
        if (targetTransform == null)
            return;

        Vector3 followPos = targetTransform.position;
        followPos.z = -10;

        Vector2 cameraSize = GetCameraSize();
        float confinerXSize = MathF.Abs(confinerXBounds.x) + MathF.Abs(confinerXBounds.y);
        float confinerYSize = MathF.Abs(confinerYBounds.x) + MathF.Abs(confinerYBounds.y);

        if (cameraSize.x > confinerXSize) 
        {
            followPos.x = confinerXBounds.y + confinerXBounds.x;
        }
        else 
        {
            followPos.x = MathF.Max(followPos.x, confinerXBounds.x + cameraSize.x / 2);
            followPos.x = MathF.Min(followPos.x, confinerXBounds.y - cameraSize.x / 2);
        }

        if (cameraSize.y > confinerYSize)
        {
            followPos.y = confinerYBounds.y + confinerYBounds.x;
        }
        else 
        {
            followPos.y = MathF.Max(followPos.y, confinerYBounds.x + cameraSize.y / 2);
            followPos.y = MathF.Min(followPos.y, confinerYBounds.y - cameraSize.y / 2);
        }

        transform.position = followPos;
    }

    private void SetCameraSize(int width, int height) 
    {
        ppCamera.refResolutionX = width;
        ppCamera.refResolutionY = height;

        //confiner2D.InvalidateCache();
    }

    public static void SetFollowTarget(Transform target) 
    {
        instance.targetTransform = target;
    }

    private Vector2 GetCameraSize()
    {
        Vector2 cameraSize = Vector2.zero;

        cameraSize.x = ppCamera.refResolutionX;
        cameraSize.y = ppCamera.refResolutionY;
        cameraSize /= pixelPerUnit;

        return cameraSize;
    }

    private void UpdateWindowPos() 
    {
        Vector2 floatMod = (transform.position * pixelPerUnit);
        Vector2Int pixelMod = new Vector2Int(Mathf.RoundToInt(floatMod.x), Mathf.RoundToInt(-floatMod.y));
        
        pixelMod += GameWindowManager.GetScreenCenterPos();

        Vector2Int dif = pixelMod - GameWindowManager.GetWindowCenterPos();

        if (dif.magnitude < minWindowUpdateDifference)
            return;

        GameWindowManager.SetWindowCenterPos(pixelMod.x, pixelMod.y);
    }
}
