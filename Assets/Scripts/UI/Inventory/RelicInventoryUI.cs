using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInventoryUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;
    [SerializeField] private RelicSlot _slotPrefab;
    [SerializeField] private Transform _slotHolder;

    private void Start()
    {
        _channel.initRelicInventoryUI += Init;
        _channel.updateRelicSlot += UpdateSlot;
    }

    public void Init(List<RelicSO> relics)
    {
        for (int i = 0; i < relics.Count; i++)
            Instantiate(_slotPrefab, _slotHolder).UpdateUI(relics[i]);
    }

    private void UpdateSlot(int index, RelicSO relic)
    {
        _slotHolder.GetChild(index).GetComponent<RelicSlot>().UpdateUI(relic);
    }
}
