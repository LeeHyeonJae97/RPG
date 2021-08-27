using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CombatEventChannel", menuName = "ScriptableObject/Event/CombatEventChannel")]
public class CombatEventChannelSO : BaseEventChannelSO
{
    public UnityAction restartStage;
    public UnityAction clearStage;
}
