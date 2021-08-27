using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UserDataSO _userData;
    [SerializeField] private CombatManager _combatManager;

    private void Start()
    {
        Addressables.LoadAssetAsync<StageSO>($"Stage_{_userData.stageId}").Completed += (handle) =>
        {
            _combatManager.CombatantJoined();
            _combatManager.StartStage(handle.Result);
        };
    }
}
