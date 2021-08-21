using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInventorySO<T> : ScriptableObject
{
    [field: SerializeField] public List<T> Items { get; protected set; } = new List<T>();

    public abstract void Load();
    public abstract void Save();
}
