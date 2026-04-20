using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzle : BasePuzzle
{
    [SerializeField] private ValveCategory[] categories;

    private void Start()
    {
        foreach (ValveCategory category in categories)
        {
            category.Initialize();
        }
    }

    protected override bool CheckSolution()
    {
        foreach (var category in categories)
        {
            if (!category.Validate())
            {
                return false;
            }
        }
        AudioManager.Instance.Play("Agua");
        return true;
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
        PlayValveSound();
    }

    public void OnValveDeactivated()
    {
        currentActivatedValves--;
        PlayValveSound();
    }

    public void PlayValveSound()
    {
        AudioManager.Instance.Play("Valvula");
    }
}