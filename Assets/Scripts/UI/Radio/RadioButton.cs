using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RadioButton : Selectable, IPointerClickHandler
{
    [Space(10)]
    public UnityEvent<bool> onStateChanged;
    public UnityEvent onSelected;
    public UnityEvent onDeselected;
    public UnityAction onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void OnStateChanged(bool value)
    {
        onStateChanged?.Invoke(value);
        if(value)
        {
            targetGraphic.color = colors.normalColor;
            onSelected?.Invoke();
            
        }
        else
        {
            targetGraphic.color = colors.selectedColor;
            onDeselected?.Invoke();
        }       
    }

    public override void OnSelect(BaseEventData eventData) { }
}

