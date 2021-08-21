using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CombatEventChannel", menuName = "ScriptableObject/Event/CombatEventChannel")]
public class CombatEventChannelSO : BaseEventChannelSO
{
    public UnityAction restartStage;
    public UnityAction clearStage;

    //public OnHpChanged onDamaged;
    //public OnHpChanged onHealed;
    //public OnStatChanged onBuffed;
    //public OnStatChanged onDebuffed;
    //public OnCrowdControlApplied onCrowdControlApplied;
}

//public delegate void OnHpChanged(string targetTag, CombatTarget type, int count, int amount);
//public delegate void OnStatChanged(string targetTag, CombatTarget type, int count, string targetStatName, int amount);
//public delegate void OnCrowdControlApplied(string targetTag, CombatTarget type, int count, CrowdControlType cc, float duration);
