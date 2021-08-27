using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataLoader : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private GameSceneSO _title;
    [SerializeField] private GameSceneSO _gameUI;
    [SerializeField] private GameSceneSO _gamePlay;

    [Header("Data")]
    [SerializeField] private UserDataSO _userData;
    [SerializeField] private CorpsSO _corps;
    [SerializeField] private QuestTableSO _questTable;
    [SerializeField] private CharacterInventorySO _characterInventory;
    [SerializeField] private EquipmentInventorySO _equipmentInventory;
    [SerializeField] private SkillInventorySO _skillInventory;
    [SerializeField] private RuneInventorySO _runeInventory;
    [SerializeField] private RelicInventorySO _relicInventory;

    public void LoadData()
    {
        StartCoroutine(LoadCoroutine());
    }

    private IEnumerator LoadCoroutine()
    {
        // 필요하다면 번들 다운로드

        // 구글 로그인 혹은 게스트 로그인

        // UI씬 로드
        AsyncOperationHandle handle = _gameUI.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        yield return new WaitUntil(() => handle.IsDone);

        // 데이터 로드
        Waiting.Begin?.Invoke("데이터 로드 중...");

        _userData.Load();
        _questTable.Load();
        _characterInventory.Load();
        _equipmentInventory.Load();
        _skillInventory.Load();
        _runeInventory.Load();
        _relicInventory.Load();
        _corps.Load(0, _characterInventory.Items, _equipmentInventory.Items, _skillInventory.Items);

        Waiting.Begin?.Invoke("씬 로드 중...");

        // Play씬 로드 후 Title씬 언로드
        _gamePlay.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += (obj) =>
        {
            _title.sceneReference.UnLoadScene();
            Waiting.End?.Invoke();
        };
    }
}
