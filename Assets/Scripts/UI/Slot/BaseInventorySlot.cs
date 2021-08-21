using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseInventorySlot<T> : MonoBehaviour
{
    [SerializeField] protected Image _previewImage;
    [SerializeField] protected Button _slotButton;

    public void Init(T t, UnityAction onClick)
    {
        UpdateUI(t);
        _slotButton.onClick.AddListener(onClick);
    }

    public abstract void UpdateUI(T t);    
}
