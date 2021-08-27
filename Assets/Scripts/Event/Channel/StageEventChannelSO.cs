using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StageEventChannel", menuName = "ScriptableObject/Event/StageEventChannel")]
public class StageEventChannelSO : BaseEventChannelSO
{
    public UnityAction<Sprite> setBackground;
    public UnityAction<StageSO> startStage;
}
