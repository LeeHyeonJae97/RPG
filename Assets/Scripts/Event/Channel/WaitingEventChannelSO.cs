using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WaitingEventChannel", menuName = "ScriptableObject/Event/WaitingEventChannel")]
public class WaitingEventChannelSO : BaseEventChannelSO
{
    public UnityAction<string> begin;
    public UnityAction end;
}
