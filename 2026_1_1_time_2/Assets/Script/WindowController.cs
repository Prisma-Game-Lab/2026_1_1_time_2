using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

//Needs Serious Refactoring

public class windowMod : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, ref Rect rectangle);
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    [DllImport("user32.dll")]
    static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg,
                                        IntPtr wParam, IntPtr lParam);

    delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    // HWND WINAPI GetForegroundWindow(void);
    string word;
    bool onEditor;

    [SerializeField] private float reduceTime;
    [SerializeField] private int startWidth;
    [SerializeField] private int startHeight;
    float timer = 0;

    Vector2Int currentSize;
    IntPtr windowHandler;

    IntPtr originalWndProc;
    WndProcDelegate newWndProc;

    [SerializeField] private GameObject test;

    private bool movingWindow;

    void Start()
    {

#if UNITY_EDITOR
        onEditor = true;
#endif

        if (onEditor)
            return;

        windowHandler = GetActiveWindow();

        newWndProc = CustomWndProc;
        const int GWLP_WNDPROC = -4;
        originalWndProc = SetWindowLongPtr(windowHandler, GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(newWndProc));

        currentSize = new Vector2Int(startWidth, startHeight);
        SetWindowSize(currentSize.x, currentSize.y);
        

        //const uint SWP_NOMOVE = 0X2;
        //const uint SWP_NOSIZE = 1;
        //const uint SWP_NOZORDER = 0X4;
        //const uint SWP_SHOWWINDOW = 0x0040;
        //const uint SWP_HIDEWINDOW = 0x0080;
        //word = GetForegroundWindow().ToString();
        //bool a = SetWindowPos(GetForegroundWindow(), 0, 0, 0, 100, 100, SWP_NOZORDER | SWP_SHOWWINDOW);
    }

    private void OnDestroy()
    {
        const int GWLP_WNDPROC = -4;
        SetWindowLongPtr(windowHandler, GWLP_WNDPROC, originalWndProc);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > reduceTime)
        {
            if (movingWindow)
                return;

            timer -= reduceTime;

            currentSize.x -= 2;
            currentSize.y -= 2;

            SetWindowSize(currentSize.x, currentSize.y);
        }
    }

    public void SetWindowSize(int width, int height) 
    {
        if (onEditor)
            return;

        const uint SWP_NOZORDER = 0X4;
        const uint SWP_SHOWWINDOW = 0x0040;
        const uint SWP_NOMOVE = 0X2;
        uint flags = SWP_NOZORDER | SWP_SHOWWINDOW;

        if (movingWindow) 
        {
            flags = flags | SWP_NOMOVE;
        }

        Vector2Int winPos = GetWindowPos();
        Vector2Int winSize = GetWindowSize();

        int dx = (winSize.x - width) / 2;
        int dy = (winSize.y - height) / 2;

        winPos.x += dx;
        winPos.y += dy;

        SetWindowPos(windowHandler, 0, winPos.x, winPos.y, width, height, flags);
    }

    public Vector2Int GetWindowPos() 
    {
        Rect windowRect = GetWindowRect();
        
        Vector2Int pos = new Vector2Int(windowRect.Left, windowRect.Top);

        return pos;
    }

    public Vector2Int GetWindowSize()
    {
        Rect windowRect = GetWindowRect();

        Vector2Int size = new Vector2Int();

        size.x = windowRect.Right - windowRect.Left;
        size.y = windowRect.Bottom - windowRect.Top;

        return size;
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

    private IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        const int WM_ENTERSIZEMOVE = 0x0231;
        const int WM_EXITSIZEMOVE = 0x0232;

        if (msg == WM_ENTERSIZEMOVE) 
        {
            movingWindow = true;
            test.SetActive(true);
        }

        if (msg == WM_EXITSIZEMOVE) 
        {
            movingWindow = false;
            test.SetActive(false);
        }

        return CallWindowProc(originalWndProc, hWnd, msg, wParam, lParam);
    }
}
