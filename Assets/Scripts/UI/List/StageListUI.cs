using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageListUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;

    [SerializeField] private StageSlot _slotPrefab;
    [SerializeField] private Transform _slotHolder;

    private void Start()
    {
        _channel.openStageListUI += Open;
    }

    private void OnDestroy()
    {
        _channel.openStageListUI -= Open;
    }

    private void Open(StageSO[] infos, UnityAction<int> selectStage)
    {        
        for (int i = 0; i < infos.Length; i++)
        {
            if (i < _slotHolder.childCount)
            {
                int index = i;
                _slotHolder.GetChild(index).GetComponent<StageSlot>().Init(infos[index], () => selectStage(infos[index].Id));
            }
            else
            {
                int index = i;
                Instantiate(_slotPrefab, _slotHolder).Init(infos[index], () => selectStage(infos[index].Id));
            }
        }
        for (int i = infos.Length; i < _slotHolder.childCount; i++)
            _slotHolder.GetChild(i).gameObject.SetActive(false);

        gameObject.SetActive(true);
    }
}
