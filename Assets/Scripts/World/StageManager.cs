using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class StageManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;

    [SerializeField] private UserDataSO _userData;
    [SerializeField] private CombatManager _combatManager;
    [SerializeField] private UIEventChannelSO _channel;

    private bool _isInit;

    private Dictionary<string, AsyncOperationHandle<IList<StageSO>>> _handleDic = new Dictionary<string, AsyncOperationHandle<IList<StageSO>>>();
    private AsyncOperationHandle<StageSO> _curStageLoadHandle = default;
    private AsyncOperationHandle<IList<MonsterSO>> _stageMonstersLoadHandle = default;

    private void Start()
    {
        SelectStage(0); // _userData.stage
    }

    public void Open()
    {
        if (!_isInit)
        {
            _isInit = true;
            StartCoroutine(InitCoroutine());
        }
        else
        {
            _channel.openWorldListUI?.Invoke(null, null);
        }
    }

    private IEnumerator InitCoroutine()
    {
        AsyncOperationHandle<IList<WorldSO>> handle = Addressables.LoadAssetsAsync<WorldSO>("Worlds", null);
        yield return new WaitUntil(() => handle.IsDone);

        _channel.openWorldListUI?.Invoke(handle.Result.ToArray(), SelectWorld);
        Addressables.Release(handle);
    }

    public void Close()
    {
        AsyncOperationHandle<IList<StageSO>>[] handles = _handleDic.Values.ToArray();
        for (int i = 0; i < handles.Length; i++)
            Addressables.Release(handles[i]);
        _handleDic = new Dictionary<string, AsyncOperationHandle<IList<StageSO>>>();

        _channel.closeWorldListUI?.Invoke();
    }

    public void SelectWorld(string label)
    {
        StartCoroutine(SelectWorldCoroutine("Stages"));
    }

    private IEnumerator SelectWorldCoroutine(string label)
    {
        if (!_handleDic.ContainsKey("Stages"))
        {
            AsyncOperationHandle<IList<StageSO>> handle = Addressables.LoadAssetsAsync<StageSO>(label, null);
            yield return new WaitUntil(() => handle.IsDone);

            _handleDic.Add(label, handle);
            _channel.openStageListUI?.Invoke(handle.Result.ToArray(), SelectStage);
        }
        else
        {
            _channel.openStageListUI?.Invoke(_handleDic[label].Result.ToArray(), SelectStage);
        }
    }

    public void SelectStage(int id)
    {
        StartCoroutine(SelectStageCoroutine(id));
    }

    private IEnumerator SelectStageCoroutine(int id)
    {
        if (_curStageLoadHandle.IsValid()) Addressables.Release(_curStageLoadHandle);
        if (_stageMonstersLoadHandle.IsValid()) Addressables.Release(_stageMonstersLoadHandle);

        _curStageLoadHandle = Addressables.LoadAssetAsync<StageSO>($"Stage_{id}");
        yield return new WaitUntil(() => _curStageLoadHandle.IsDone);

        StageSO stage = _curStageLoadHandle.Result;

        _stageMonstersLoadHandle = Addressables.LoadAssetsAsync<MonsterSO>(stage.Name, null);
        yield return new WaitUntil(() => _stageMonstersLoadHandle.IsDone);


        _combatManager.InitStage(stage, _stageMonstersLoadHandle.Result.ToArray());

        _sr.sprite = stage.Background;
        _channel.updateStageUI?.Invoke(stage.Name);

        Close();
    }
}
