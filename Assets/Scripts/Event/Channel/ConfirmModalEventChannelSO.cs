using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ConfirmModalEventChannel", menuName = "ScriptableObject/Event/ConfirmModalEventChannel")]
public class ConfirmModalEventChannelSO : BaseEventChannelSO
{
    public UnityAction<string, UnityAction> show;
}
