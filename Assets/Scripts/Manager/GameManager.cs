using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using TigerForge;
using static Variables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSceneSO _title;
    [SerializeField] private GameSceneSO _game;

    [SerializeField] private UserDataSO _userData;
    [SerializeField] private CorpsSO _corps;
    [SerializeField] private QuestTableSO _questTable;
    [SerializeField] private CharacterInventorySO _characterInventory;
    [SerializeField] private EquipmentInventorySO _equipmentInventory;
    [SerializeField] private SkillInventorySO _skillInventory;
    [SerializeField] private RuneInventorySO _runeInventory;
    [SerializeField] private RelicInventorySO _relicInventory;

    [SerializeField] private UIEventChannelSO _uiEventChannel;
    [SerializeField] private WaitingEventChannelSO _waitingEventChannel;

    public void LoadData()
    {
        _waitingEventChannel.begin?.Invoke("데이터 로드 중...");

        _userData.Load();
        _questTable.Load();
        _characterInventory.Load();
        _equipmentInventory.Load();
        _skillInventory.Load();
        _runeInventory.Load();
        _relicInventory.Load();
        _corps.Load(0, _characterInventory.Items, _equipmentInventory.Items, _skillInventory.Items);

        _game.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += (obj) => _title.sceneReference.UnLoadScene();

        _waitingEventChannel.end?.Invoke();
    }
}
