using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseInventorySlot<T> : MonoBehaviour
{
    [SerializeField] protected Image _previewImage;
    [SerializeField] protected Button _slotButton;

    public abstract void UpdateUI(T t);

    public void Init(UnityAction onClick)
    {
        GetComponent<Button>().onClick.AddListener(onClick);
    }  

    public void Remove()
    {
        Destroy(gameObject);
    }
}
