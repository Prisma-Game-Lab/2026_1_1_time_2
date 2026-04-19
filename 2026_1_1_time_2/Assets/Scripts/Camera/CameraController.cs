using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("References")]
    [SerializeField] private PixelPerfectCamera ppCamera;
    [SerializeField] private CameraFollowStrategy playerFollowStrategy;
    [SerializeField] private CameraFollowStrategy mouseFollowStrategy;

    [Header("Variables")]
    [SerializeField] private FollowStrategy defaultFollowStrategy;
    [SerializeField] private float minWindowUpdateDistance;
    [SerializeField] private float minWindowUpdateDifference;

    private Vector2 confinerXBounds;
    private Vector2 confinerYBounds;
    private float pixelPerUnit = 100;

    private Vector2 previousPos;

    private CameraFollowStrategy currentFollowStrategy;

    enum FollowStrategy 
    {
        Player, Mouse
    }

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
        switch (defaultFollowStrategy) 
        {
            case FollowStrategy.Player:
                currentFollowStrategy = playerFollowStrategy;
                break;
            case FollowStrategy.Mouse:
                currentFollowStrategy = mouseFollowStrategy;
                break;
        }

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
        currentFollowStrategy.FollowTarget();

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

    public static void FollowPlayer() 
    {
        instance.currentFollowStrategy = instance.playerFollowStrategy;
    }

    public static void FollowMouse() 
    {
        instance.currentFollowStrategy = instance.mouseFollowStrategy;
    }

    public Vector2 GetConfinerXBound() 
    {
        return confinerXBounds;
    }

    public Vector2 GetConfinerYBound() 
    {
        return confinerYBounds;
    }

    private void SetCameraSize(int width, int height) 
    {
        ppCamera.refResolutionX = width;
        ppCamera.refResolutionY = height;

        //confiner2D.InvalidateCache();
    }

    public Vector2 GetCameraSize()
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
