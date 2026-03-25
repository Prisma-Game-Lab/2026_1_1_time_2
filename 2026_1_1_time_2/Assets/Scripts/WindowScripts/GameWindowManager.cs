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

    [SerializeField] private bool fullScreenWindow;
    [SerializeField] private Vector2Int defaultWindowSize;

    //True means starting drag
    //False means ending drag
    [SerializeField] public UnityEvent<bool> OnWindowDragging;
    [SerializeField] public UnityEvent<int, int> OnWindowPosChange;
    [SerializeField] public UnityEvent<int, int> OnWindowSizeChange;

    private IntPtr windowHandler;
    private IntPtr originalWindowProcessor;
    private WndProcDelegate newWindowProcessor;

    private bool editorMode;
    private bool draggingWindow;

    private Vector2Int windowSize = -Vector2Int.one;
    private Vector2Int windowPos = -Vector2Int.one;
    private Vector2Int screenCenterPos = -Vector2Int.one;

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

        if (!editorMode) 
        {
            const int GWLP_WNDPROC = -4;
            
            newWindowProcessor = CustomWindowProcessor;
            originalWindowProcessor = SetWindowLongPtr(windowHandler, GWLP_WNDPROC,
                                                       Marshal.GetFunctionPointerForDelegate(newWindowProcessor));
        }

        windowSize = GetWindowSize();
        windowPos = GetWindowPos();
        screenCenterPos = GetScreenCenterPos();

        SetDefaultWindowState();
    }

    private void OnDestroy()
    {
        //SetDefaultWindowState();

        if (!editorMode)
        {
            const int GWLP_WNDPROC = -4;

            SetWindowLongPtr(windowHandler, GWLP_WNDPROC, originalWindowProcessor);
        }
    }

    public static Vector2Int GetWindowPos()
    {
        //if (instance.windowPos != -Vector2Int.one)
        //    return instance.windowPos;

        Rect windowRect = instance.GetWindowRect();

        Vector2Int pos = new Vector2Int(windowRect.Left, windowRect.Top);

        return pos;
    }

    public static Vector2Int GetWindowCenterPos()
    {
        Rect windowRect = instance.GetWindowRect();

        Vector2Int pos = new Vector2Int(windowRect.Left, windowRect.Top);

        pos.x += (windowRect.Right - windowRect.Left) / 2;
        pos.y += (windowRect.Bottom - windowRect.Top) / 2;

        return pos;
    }

    public static Vector2Int GetScreenCenterPos() 
    {
        //if (instance.screenCenterPos != -Vector2Int.one)
        //    return instance.screenCenterPos;

        Vector2Int pos = GetScreenSize();
        pos /= 2;

        return pos;
    }

    public static Vector2Int GetWindowSize()
    {
        //if (instance.windowSize != -Vector2Int.one)
        //    return instance.windowSize;

        Rect windowRect = instance.GetWindowRect();

        Vector2Int size = new Vector2Int();

        size.x = windowRect.Right - windowRect.Left;
        size.y = windowRect.Bottom - windowRect.Top;

        return size;
    }

    public static Vector2Int GetScreenSize()
    {
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        Vector2Int screenSize = new Vector2Int(screenWidth, screenHeight);

        return screenSize;
    }

    public static void SetWindowPos(int x, int y)
    {
        if (instance.editorMode || instance.draggingWindow)
            return;

        uint flags = SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW;

        print($"Window Desired Pos: {x} {y}");

        Vector2Int winPos = ClampWindowPos(x, y);

        print($"Window Clamped Pos: {winPos}");

        instance.windowPos = winPos;

        SetWindowPos(instance.windowHandler, 0, winPos.x, winPos.y, 0, 0, flags);

        instance.OnWindowPosChange.Invoke(winPos.x, winPos.y);
    }

    public static void SetWindowCenterPos(int x, int y)
    {
        if (instance.editorMode || instance.draggingWindow)
            return;

        Vector2Int windowSize = GetWindowSize();

        print($"Window Size: {windowSize}");

        x -= windowSize.x / 2;
        y -= windowSize.y / 2;

        SetWindowPos(x, y);
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

        instance.windowSize.x = width;
        instance.windowSize.y = height;

        Vector2Int clampedPos = ClampWindowPos(winPos);
        if (dx < 0)
            winPos.x = clampedPos.x;
        if (dy < 0)
            winPos.y = clampedPos.y;

        instance.windowPos = winPos;

        instance.OnWindowSizeChange.Invoke(width, height);
        SetWindowPos(instance.windowHandler, 0, winPos.x, winPos.y, width, height, flags);
    }

    public static void CenterWindow() 
    {
        Vector2Int centerPos = GetScreenCenterPos();

        print($"Center Pos: {centerPos}");

        SetWindowCenterPos(centerPos.x, centerPos.y);
    }

    public static Vector2Int GetDefaultWindowSize() 
    {
        return instance.defaultWindowSize;
    }

    private void SetDefaultWindowState() 
    {
        if (fullScreenWindow)
        {
            Vector2Int screenSize = GetScreenSize();
            SetWindowSize(screenSize.x + 16, screenSize.y + 8);
            print($"Default Window Pos: {GetWindowPos()}");
            //SetWindowSize(screenSize.x, screenSize.y - 2);
        }
        else
        {
            SetWindowSize(defaultWindowSize.x, defaultWindowSize.y);
        }
        CenterWindow();
        print($"After Center: {GetWindowPos()}");
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

    private IntPtr CustomWindowProcessor(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        const int WM_CLOSE = 0x0010;
        const int WM_ENTERSIZEMOVE = 0x0231;
        const int WM_EXITSIZEMOVE = 0x0232;

        if (msg == WM_CLOSE)
        {
            const int GWLP_WNDPROC = -4;

            SetWindowLongPtr(windowHandler, GWLP_WNDPROC, originalWindowProcessor);
        }

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

    private static Vector2Int ClampWindowPos(int x, int y) 
    {
        Vector2Int windowSize = GetWindowSize();
        Vector2Int screenBounds = GetScreenSize() - windowSize;

        Vector2Int clampedPos = new Vector2Int(x, y);

        if (screenBounds.x > 0) 
        {
            clampedPos.x = Mathf.Max(0, clampedPos.x);
            clampedPos.x = Mathf.Min(clampedPos.x, screenBounds.x);
        }
        else 
        {
            clampedPos.x = screenBounds.x / 2;
        }

        if (screenBounds.y > 0)
        {
            clampedPos.y = Mathf.Max(0, clampedPos.y);
            clampedPos.y = Mathf.Min(clampedPos.y, screenBounds.y);
        }
        else 
        {
            clampedPos.y = screenBounds.y / 2;
        }

        print($"Clamped Pos: {clampedPos}");

        return clampedPos;
    }

    private static Vector2Int ClampWindowPos(Vector2Int clampedPos)
    {
        return ClampWindowPos(clampedPos.x, clampedPos.y);
    }
}
