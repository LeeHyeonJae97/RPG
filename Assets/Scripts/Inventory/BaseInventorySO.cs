using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInventorySO<T> : ScriptableObject
{
    public List<T> Items { get; protected set; } = new List<T>();

    public UnityAction<T> onAdded;
    public UnityAction<T> onRemoved;

    public abstract void Load();
    public abstract void Save();

    public void Add(T item)
    {
        Items.Add(item);
        onAdded?.Invoke(item);
    }

    public void Remove(T item)
    {
        Items.Remove(item);
        onRemoved?.Invoke(item);
    }
}
