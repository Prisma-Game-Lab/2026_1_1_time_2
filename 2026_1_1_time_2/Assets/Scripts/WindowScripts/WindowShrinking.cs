using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class WindowShrinking : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private bool startOnAwake;
    [SerializeField] private bool setSizeOnAwake;

    [Header("Variables")]
    [SerializeField] private Vector2Int startWindowSize;
    [SerializeField] private Vector2Int shrinkingModifier;
    [SerializeField] private float shrinkUnitTime;
    [SerializeField] private Vector2Int deathMinWindowSize;
    [SerializeField] private Vector2Int deathShrinkingModifier;
    [SerializeField] private float deathShrinkUnitTime;

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
        if (deathSequence)
            return;

        currentShrinkTime = 0;
        shrinking = true;
        GameWindowManager.instance.OnWindowMinSizeReached.AddListener(ActivateDeathShrinking);
    }

    public void StopShrinking() 
    {
        if (deathSequence)
            return;

        shrinking = false;
        GameWindowManager.instance.OnWindowMinSizeReached.RemoveListener(ActivateDeathShrinking);
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
            if (deathSequence)
                print("a");

            currentShrinkTime -= shrinkUnitTime;
            
            Vector2Int currentSize = GameWindowManager.GetWindowSize();
            Vector2Int newSize = currentSize - shrinkingModifier;
            GameWindowManager.SetWindowSize(newSize.x, newSize.y);
        }
    }

    private void ActivateDeathShrinking() 
    {
        print("Death Sequence");
        deathSequence = true;
        shrinkUnitTime = deathShrinkUnitTime;
        shrinkingModifier = deathShrinkingModifier;
        print(deathMinWindowSize);
        GameWindowManager.SetMinWindowSize(deathMinWindowSize);

        GameWindowManager.instance.OnWindowMinSizeReached.RemoveListener(ActivateDeathShrinking);
        GameWindowManager.instance.OnWindowMinSizeReached.AddListener(OnWindowDeath);
    }

    private void OnWindowDeath() 
    {
        GameWindowManager.instance.OnWindowMinSizeReached.RemoveListener(OnWindowDeath);

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
