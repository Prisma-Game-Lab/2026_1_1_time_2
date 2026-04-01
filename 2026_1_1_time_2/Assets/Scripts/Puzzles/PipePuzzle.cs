using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzle : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private ValveCategory[] categories;
    [SerializeField] private Transform mouseObject;

    private void Start()
    {
        foreach (ValveCategory category in categories)
        {
            category.Initialize();
        }
    }

    public void ActivatePuzzle() 
    {
        canvas.SetActive(true);
        CameraController.SetFollowTarget(mouseObject);
    }

    public void DeactivatePuzzle() 
    {
        canvas.SetActive(false);
    }

    public void ValidateAnswer() 
    {
        foreach (var category in categories)
        {
            if (!category.Validate())
            {
                print("Incorrect Solution");
                return;
            }
        }

        print("Right Solution");
    }
}

[Serializable]
public class ValveCategory 
{
    public int amountNeeded;
    public ValveScript[] valves;

    private int currentActivatedValves;

    public void Initialize() 
    {
        foreach (ValveScript valve in valves) 
        {
            valve.Initialize(this);
        }
    }

    public bool Validate() 
    {
        if (currentActivatedValves != amountNeeded)
        {
            return false;
        }

        return true;
    }

    public void OnValveActivated() 
    {
        currentActivatedValves++;
    }

    public void OnValveDeactivated()
    {
        currentActivatedValves--;
    }
}