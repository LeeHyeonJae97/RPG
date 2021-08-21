using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TooltipEventChannel", menuName = "ScriptableObject/Event/TooltipEventChannel")]
public class TooltipEventChannelSO : BaseEventChannelSO
{
    public UnityAction<System.Object> show;
    public UnityAction hide;
    public UnityAction follow;
}
