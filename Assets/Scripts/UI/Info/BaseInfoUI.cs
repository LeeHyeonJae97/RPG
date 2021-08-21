using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInfoUI<T> : MonoBehaviour
{
    [SerializeField] protected UIEventChannelSO _channel;

    public virtual void Open(T t)
    {
        UpdateUI(t);
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public abstract void UpdateUI(T t);
}
