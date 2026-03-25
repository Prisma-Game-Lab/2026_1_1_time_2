using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindowShrinking : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private bool startOnAwake;
    [SerializeField] private bool setSizeOnAwake;

    [Header("Variables")]
    [SerializeField] private Vector2Int startWindowSize;
    [SerializeField] private Vector2Int minWindowSize;
    [SerializeField] private Vector2Int shrinkingModifier;
    [SerializeField] private float shrinkUnitTime;
    [SerializeField] private Vector2Int deathMinWindowSize;
    [SerializeField] private Vector2Int deathShrinkingModifier;
    [SerializeField] private float deathShrinkUnitTime;

    [Header("Events")]
    [SerializeField] private UnityEvent onMinSizeReached;

    private float currentShrinkTime;

    private bool shrinking;
    private bool deathSequence;
    private bool paused;
    private bool editorMode;

    private void Start()
    {
        #if UNITY_EDITOR
                editorMode = true;
        #endif

        if (setSizeOnAwake) 
        {
            GameWindowManager.SetWindowSize(startWindowSize.x, startWindowSize.y);
        }

        if (startOnAwake)
        {
            StartShrinking();
        } 
    }

    private void Update()
    {
        if (shrinking) 
        {
            ShrinkingStep();
        }
    }

    private void OnEnable()
    {
        GameWindowManager.instance.OnWindowDragging.AddListener(SetPause);
    }

    private void OnDisable()
    {
        GameWindowManager.instance.OnWindowDragging.RemoveListener(SetPause);
    }

    private void OnDestroy()
    {
        StopShrinking();
    }

    public void StartShrinking() 
    {

        currentShrinkTime = 0;

        shrinking = true;
    }

    public void StopShrinking() 
    {
        shrinking = false;
    }

    public void SetPause(bool state) 
    {
        if (!shrinking)
            return;

        paused = state;
    }

    private void ShrinkingStep() 
    {
        if (paused)
            return;

        currentShrinkTime += Time.deltaTime;
        if (currentShrinkTime > shrinkUnitTime) 
        {
            currentShrinkTime -= shrinkUnitTime;
            
            Vector2Int currentSize = GameWindowManager.GetWindowSize();
            Vector2Int newSize = currentSize - shrinkingModifier;
            newSize = new Vector2Int(Mathf.Max(minWindowSize.x, newSize.x), Mathf.Max(minWindowSize.y, newSize.y));
            GameWindowManager.SetWindowSize(newSize.x, newSize.y);

            CheckForMinSize();
        }
    }

    private void CheckForMinSize() 
    {
        Vector2Int currentSize = GameWindowManager.GetWindowSize();

        if (currentSize.x <= minWindowSize.x || currentSize.y <= minWindowSize.y) 
        {
            if (!deathSequence)
            {
                deathSequence = true;
                onMinSizeReached.Invoke();
                ActivateDeathShrinking();
            }
            else
            {
                if (editorMode)
                {
                    print("Morreu");
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    }

    private void ActivateDeathShrinking() 
    {
        shrinkUnitTime = deathShrinkUnitTime;
        shrinkingModifier = deathShrinkingModifier;
        minWindowSize = deathMinWindowSize;
    }
}
