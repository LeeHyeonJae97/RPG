using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    [SerializeField] private GameSceneSO _global;
    [SerializeField] private GameSceneSO _title;

    private IEnumerator Start()
    {
        AsyncOperationHandle handle1 = _global.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        AsyncOperationHandle handle2 = _title.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        yield return new WaitUntil(() => handle1.IsDone && handle2.IsDone);

        SceneManager.UnloadSceneAsync(0);
    }
}
