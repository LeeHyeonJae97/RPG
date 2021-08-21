using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "LoadingEventChannel", menuName = "ScriptableObject/Event/LoadingEventChannel")]
public class LoadingEventChannelSO : BaseEventChannelSO
{
    public UnityAction<Loading> add;
    public UnityAction load;

    public UnityAction<int> updateProgressUI;
    public UnityAction<string> updateDescriptionUI;
}
