using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class StageUI : MonoBehaviour
{
    [SerializeField] private UserDataSO _userData;

    [SerializeField] private WorldSlot _worldSlotPrefab;
    [SerializeField] private Transform _worldSlotHolder;

    [SerializeField] private StageSlot _stageSlotPrefab;
    [SerializeField] private Transform _stageSlotHolder;

    [SerializeField] private StageEventChannelSO _channel;

    private AsyncOperationHandle<IList<WorldSO>> _handle;
    private Dictionary<string, AssetReference[]> _stageDic = new Dictionary<string, AssetReference[]>();

    public void Open()
    {
        _handle = Addressables.LoadAssetsAsync<WorldSO>("Worlds", null);
        _handle.Completed += (handle) => OpenWorld(handle.Result.ToArray(), SelectWorld);
    }

    public void Close()
    {
        Addressables.Release(_handle);

        AssetReference[][] stages = _stageDic.Values.ToArray();
        for (int i = 0; i < stages.GetLength(0); i++)
            Extension.Addressables.ReleaseAssets(stages[i]);

        gameObject.SetActive(false);
    }

    private async void SelectWorld(string world, AssetReference[] refs)
    {
        if (!_stageDic.ContainsKey(world))
        {
            AsyncOperationHandle<StageSO>[] handles = Extension.Addressables.LoadAssetsAsync<StageSO>(refs, null);
            await Extension.Addressables.WaitAsync<StageSO>(handles);

            _stageDic.Add(world, refs);
        }

        OpenStage(_stageDic[world], SelectStage);
    }

    public void SelectStage(AssetReference reference)
    {
        reference.LoadAssetAsync<StageSO>().Completed += (handle) =>
        {
            StageSO stage = handle.Result;

            _userData.StageName = stage.Name;
            _userData.stageId = stage.Id;
            _channel.setBackground?.Invoke(stage.Background);
            _channel.startStage?.Invoke(stage);
        };
    }

    private void OpenWorld(WorldSO[] infos, UnityAction<string, AssetReference[]> selectWorld)
    {
        if (_worldSlotHolder.childCount == 0)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                int index = i;
                Instantiate(_worldSlotPrefab, _worldSlotHolder)
                    .Init(infos[index], () => selectWorld(infos[index].Name, infos[index].Stages));
            }
        }
        else
        {
            for (int i = 0; i < infos.Length; i++)
            {
                int index = i;
                _worldSlotHolder.GetChild(i).GetComponent<WorldSlot>()
                    .Init(infos[index], () => selectWorld(infos[index].Name, infos[index].Stages));
            }
        }

        gameObject.SetActive(true);
    }

    private void OpenStage(AssetReference[] infos, UnityAction<AssetReference> selectStage)
    {
        for (int i = 0; i < infos.Length; i++)
        {
            if (i < _stageSlotHolder.childCount)
            {
                int index = i;
                _stageSlotHolder.GetChild(index).GetComponent<StageSlot>()
                    .Init(infos[index].Asset as StageSO, () => selectStage(infos[index]));
            }
            else
            {
                int index = i;
                Instantiate(_stageSlotPrefab, _stageSlotHolder)
                    .Init(infos[index].Asset as StageSO, () => selectStage(infos[index]));
            }
        }
        for (int i = infos.Length; i < _stageSlotHolder.childCount; i++)
            _stageSlotHolder.GetChild(i).gameObject.SetActive(false);

        gameObject.SetActive(true);
    }
}
