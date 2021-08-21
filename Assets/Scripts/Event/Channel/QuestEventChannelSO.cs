using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "QuestEventChannel", menuName = "ScriptableObject/Event/QuestEventChannel")]
public class QuestEventChannelSO : BaseEventChannelSO
{
    public UnityAction<string> perform;
}
