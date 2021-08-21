using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuneInventoryUI : BaseInventoryUI<Rune, RuneSlot>
{
    [SerializeField] protected IntEventChannelSO _inventoryTabEventChannel;

    private void Start()
    {
        _channel.initRuneInventoryUI += InitUI;
        _channel.addRuneSlot += AddSlot;
        _channel.removeRuneSlot += RemoveSlot;
        _channel.updateRuneSlot += UpdateSlot;
        _channel.openRuneInventoryUIWithCutout += OpenWithCutout;
    }

    private void OnDestroy()
    {
        _channel.initRuneInventoryUI -= InitUI;
        _channel.addRuneSlot -= AddSlot;
        _channel.removeRuneSlot -= RemoveSlot;
        _channel.updateRuneSlot -= UpdateSlot;
        _channel.openRuneInventoryUIWithCutout -= OpenWithCutout;
    }

    protected override void InitUI(List<Rune> runes, UnityAction<Rune> selectRune)
    {
        for (int i = 0; i < runes.Count; i++)
        {
            RuneSlot slot = Instantiate(_slotPrefab, _slotHolder[(int)runes[i].Info.Type]);
            int index = i;
            slot.Init(runes[index], () => selectRune(runes[index]));
        }
    }

    protected override void AddSlot(Rune rune, UnityAction<Rune> selectRune)
    {
        RuneSlot slot = Instantiate(_slotPrefab, _slotHolder[(int)rune.Info.Type]);
        slot.Init(rune, () => selectRune(rune));
    }

    protected override void UpdateSlot(int index, Rune rune)
    {
        _slotHolder[(int)rune.Info.Type].GetChild(index).GetComponent<RuneSlot>().UpdateUI(rune);
    }
}
