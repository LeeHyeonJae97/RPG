using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CutoutEventChannel", menuName = "ScriptableObject/Event/CutoutEventChannel")]
public class CutoutEventChannelSO : BaseEventChannelSO
{
    public UnityAction<RectTransform> show;
    public UnityAction hide;
    public UnityAction onHide;
}
