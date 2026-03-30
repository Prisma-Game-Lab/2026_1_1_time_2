using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ValveScript : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Color activeColor;

    [Header("Events")]
    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private UnityEvent OnDeactivate;

    private ValveCategory category;
    private Image image;

    private bool state;
    private Color originalColor;

    private void Start()
    {
        image = GetComponent<Image>();

        originalColor = image.color;
    }

    public void Initialize(ValveCategory category) 
    {
        this.category = category;
    }

    public void ToggleState() 
    {
        state = !state;

        if (state) 
        {
            image.color = activeColor;
            category.OnValveActivated();
            OnActivate.Invoke();
        }
        else 
        {
            image.color = originalColor;
            category.OnValveDeactivated();
            OnDeactivate.Invoke();
        }
    }

    public bool IsActive() 
    {
        return state;
    }
}
