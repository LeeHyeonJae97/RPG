using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BoolEventChannel", menuName = "ScriptableObject/Event/BoolEventChannel")]
public class BoolEventChannelSO : BaseEventChannelSO
{
    public UnityAction<bool> onEventRaised;
}
