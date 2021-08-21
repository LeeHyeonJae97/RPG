using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInventoryUI<T1, T2> : MonoBehaviour where T2 : BaseInventorySlot<T1>
{
    [SerializeField] protected UIEventChannelSO _channel;
    [SerializeField] protected CutoutEventChannelSO _cutoutEventChannel;    

    [SerializeField] protected RectTransform _panel;
    [SerializeField] protected Transform[] _slotHolder;
    [SerializeField] protected T2 _slotPrefab;

    protected void OpenWithCutout()
    {
        _cutoutEventChannel.show?.Invoke(_panel);
        _cutoutEventChannel.onHide += () => gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    protected void RemoveSlot(int holderIndex, int slotIndex)
    {
        Destroy(_slotHolder[holderIndex].GetChild(slotIndex).gameObject);
    }

    protected abstract void InitUI(List<T1> ts, UnityAction<T1> select);
    protected abstract void AddSlot(T1 t, UnityAction<T1> select);
    protected abstract void UpdateSlot(int index, T1 t);       
}
