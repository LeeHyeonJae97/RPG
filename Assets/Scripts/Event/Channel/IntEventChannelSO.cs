using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntEventChannel", menuName = "ScriptableObject/Event/IntEventChannel")]
public class IntEventChannelSO : BaseEventChannelSO
{
    public UnityAction<int> onEventRaised;
}
