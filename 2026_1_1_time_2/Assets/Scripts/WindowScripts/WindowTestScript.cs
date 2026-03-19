using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTestScript : MonoBehaviour
{
    [SerializeField] private Vector2Int sizeModifier;

    public void SizeUp() 
    {
        Vector2Int currentSize = GameWindowManager.GetWindowSize();
        Vector2Int newSize = currentSize + sizeModifier;
        GameWindowManager.SetWindowSize(newSize.x, newSize.y);
    }

    public void SizeDown()
    {
        Vector2Int currentSize = GameWindowManager.GetWindowSize();
        Vector2Int newSize = currentSize - sizeModifier;
        GameWindowManager.SetWindowSize(newSize.x, newSize.y);
    }
}
