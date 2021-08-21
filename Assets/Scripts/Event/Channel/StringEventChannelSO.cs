using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StringEventChannel", menuName = "ScriptableObject/Event/StringEventChannel")]
public class StringEventChannelSO : BaseEventChannelSO
{
    public UnityAction<string> onEventRaised;
}
