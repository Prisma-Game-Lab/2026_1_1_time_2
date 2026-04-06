using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ValveScript : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Sprite closedSprite;

    [Header("Events")]
    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private UnityEvent OnDeactivate;

    private ValveCategory category;
    private Image image;

    private bool state;
    private Sprite originalSprite;

    private void Start()
    {
        image = GetComponent<Image>();

        originalSprite = image.sprite;
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
            ActivateValve();
        }
        else 
        {
            DeactivateValve();
        }
    }

    public void ActivateValve() 
    {
        image.sprite = closedSprite;
        category.OnValveActivated();
        OnActivate.Invoke();
    }

    public void DeactivateValve()
    {
        image.sprite = originalSprite;
        category.OnValveDeactivated();
        OnDeactivate.Invoke();
    }

    public bool IsActive() 
    {
        return state;
    }
}
