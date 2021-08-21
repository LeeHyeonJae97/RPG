using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEventChannelSO : ScriptableObject
{
    [TextArea]
    [SerializeField] private string _description;
}
