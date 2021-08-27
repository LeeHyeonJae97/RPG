using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System.Linq;

namespace Extension
{
    public static class Addressables
    {
        public static AsyncOperationHandle<T>[] LoadAssetsAsync<T>(IEnumerable<AssetReference> refs, System.Action<T> callback)
        {
            AssetReference[] refArray = refs.ToArray();
            AsyncOperationHandle<T>[] handles = new AsyncOperationHandle<T>[refArray.Length];

            for (int i = 0; i < refArray.Length; i++)
            {
                handles[i] = refArray[i].LoadAssetAsync<T>();
                handles[i].Completed += (handle) => callback?.Invoke(handle.Result);
            }

            return handles;
        }

        public static void ReleaseAssets(IEnumerable<AssetReference> refs)
        {
            foreach (AssetReference reference in refs)
                UnityEngine.AddressableAssets.Addressables.Release(reference);
        }

        public static void ReleaseAssets(IEnumerable<AsyncOperationHandle> handles)
        {
            foreach (AsyncOperationHandle handle in handles)
                UnityEngine.AddressableAssets.Addressables.Release(handle);
        }

        public static void ReleaseAssets<T>(IEnumerable<T> objs)
        {
            foreach (T obj in objs)
                UnityEngine.AddressableAssets.Addressables.Release(obj);
        }

        public static IEnumerator WaitCoroutine<T>(AsyncOperationHandle<T>[] handles)
        {
            yield return new WaitUntil(() =>
            {
                for (int i = 0; i < handles.Length; i++)
                {
                    if (!handles[i].IsDone)
                        return false;
                }

                return true;
            });
        }

        public static T[] Results<T>(AsyncOperationHandle<T>[] handles)
        {
            T[] results = new T[handles.Length];
            for (int i = 0; i < handles.Length; i++)
                results[i] = handles[i].Result;

            return results;
        }

        public static async Task<T[]> WaitAsync<T>(AsyncOperationHandle<T>[] handles)
        {
            Task[] tasks = new Task[handles.Length];
            for (int i = 0; i < handles.Length; i++)
                tasks[i] = handles[i].Task;

            await Task.WhenAll(tasks);
            return Results(handles);
        }
    }
}
