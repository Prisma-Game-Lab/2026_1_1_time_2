using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class GameWindowManager : MonoBehaviour
{
    #region WindowsImport

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, ref Rect rectangle);
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    [DllImport("user32.dll")]
    static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg,
                                        IntPtr wParam, IntPtr lParam);

    delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    public const uint SWP_NOSIZE = 0x0001;
    public const uint SWP_NOMOVE = 0x0002;
    public const uint SWP_NOZORDER = 0x0004;
    public const uint SWP_SHOWWINDOW = 0x0040;
    public const uint SWP_FRAMECHANGED = 0x0020;

    #endregion

    public static GameWindowManager instance;

    [SerializeField] private Vector2Int defaultWindowSize;

    //True means starting drag
    //False means ending drag
    [SerializeField] public UnityEvent<bool> OnWindowDragging;

    private IntPtr windowHandler;
    private IntPtr originalWindowProcessor;
    private WndProcDelegate newWindowProcessor;

    private bool editorMode;
    private bool draggingWindow;

    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        #if UNITY_EDITOR
            editorMode = true;
        #endif  

        windowHandler = GetActiveWindow();

        const int GWLP_WNDPROC = -4;

        newWindowProcessor = CustomWindowProcessor;
        originalWindowProcessor = SetWindowLongPtr(windowHandler, GWLP_WNDPROC,
                                                   Marshal.GetFunctionPointerForDelegate(newWindowProcessor));

        SetWindowSize(defaultWindowSize.x, defaultWindowSize.y);
        CenterWindow();
    }

    private void OnDestroy()
    {
        SetWindowSize(defaultWindowSize.x, defaultWindowSize.y);

        const int GWLP_WNDPROC = -4;

        SetWindowLongPtr(windowHandler, GWLP_WNDPROC, originalWindowProcessor);
    }

    public static Vector2Int GetWindowPos()
    {
        Rect windowRect = instance.GetWindowRect();

        Vector2Int pos = new Vector2Int(windowRect.Left, windowRect.Top);

        return pos;
    }

    public static Vector2Int GetWindowSize()
    {
        Rect windowRect = instance.GetWindowRect();

        Vector2Int size = new Vector2Int();

        size.x = windowRect.Right - windowRect.Left;
        size.y = windowRect.Bottom - windowRect.Top;

        return size;
    }

    public static void SetWindowPos(int x, int y)
    {
        if (instance.editorMode || instance.draggingWindow)
            return;

        uint flags = SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW;

        SetWindowPos(instance.windowHandler, 0, x, y, 0, 0, flags);
    }

    public static void SetWindowSize(int width, int height)
    {
        if (instance.editorMode)
            return;

        uint flags = SWP_NOZORDER | SWP_SHOWWINDOW;

        if (instance.draggingWindow)
        {
            flags = flags | SWP_NOMOVE;
        }

        Vector2Int winPos = GetWindowPos();
        Vector2Int winSize = GetWindowSize();

        int dx = (winSize.x - width) / 2;
        int dy = (winSize.y - height) / 2;

        winPos.x += dx;
        winPos.y += dy;

        SetWindowPos(instance.windowHandler, 0, winPos.x, winPos.y, width, height, flags);
        instance.UpdateCameraSize(winSize, new Vector2Int(width, height));
    }

    public static void CenterWindow() 
    {
        Vector2Int windowSize = GetWindowSize();

        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        int centerX = (screenWidth - windowSize.x) / 2;
        int centerY = (screenHeight - windowSize.y) / 2;

        SetWindowPos(centerX, centerY);
    }

    private Rect GetWindowRect()
    {
        Rect windowRect = new Rect();

        if (windowHandler != IntPtr.Zero)
        {
            if (GetWindowRect(windowHandler, ref windowRect))
            {
                return windowRect;
            }
        }

        return windowRect;
    }

    private void UpdateCameraSize(Vector2Int previousScreenSize, Vector2Int newScreenSize) 
    {
        float cameraSize = Camera.main.orthographicSize;

        float newCameraSize = (newScreenSize.magnitude * cameraSize) / previousScreenSize.magnitude;

        Camera.main.orthographicSize = newCameraSize;
    }

    private IntPtr CustomWindowProcessor(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        const int WM_ENTERSIZEMOVE = 0x0231;
        const int WM_EXITSIZEMOVE = 0x0232;

        if (msg == WM_ENTERSIZEMOVE)
        {
            draggingWindow = true;
            OnWindowDragging.Invoke(true);
        }

        if (msg == WM_EXITSIZEMOVE)
        {
            draggingWindow = false;
            OnWindowDragging.Invoke(false);
        }

        return CallWindowProc(originalWindowProcessor, hWnd, msg, wParam, lParam);
    }
}
