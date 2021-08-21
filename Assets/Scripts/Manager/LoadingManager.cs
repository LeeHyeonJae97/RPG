using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private LoadingEventChannelSO _channel;

    private List<Loading> _loadings;

    private void Start()
    {
        _channel.add += Add;
        _channel.load += Load;
    }

    private void OnDestroy()
    {
        _channel.add -= Add;
        _channel.load -= Load;
    }

    private void Add(Loading loading)
    {
        if (_loadings == null) _loadings = new List<Loading>();
        _loadings.Add(loading);
    }

    private void Load()
    {
        StartCoroutine(LoadCoroutine());
    }

    private IEnumerator LoadCoroutine()
    {
        for (int i = 0; i < _loadings.Count; i++)
        {
            string description = _loadings[i].Description;
            AsyncOperationHandle handle = _loadings[i].Handle;

            _channel.updateDescriptionUI?.Invoke(description);

            while (!handle.IsDone)
            {
                _channel.updateProgressUI?.Invoke((int)handle.PercentComplete);
                yield return null;
            }
        }

        _loadings = null;
    }
}

public class Loading
{
    public string Description { get; private set; }
    public AsyncOperationHandle Handle { get; private set; }

    public Loading(string description, AsyncOperationHandle handle)
    {
        Description = description;
        Handle = handle;
    }
}
