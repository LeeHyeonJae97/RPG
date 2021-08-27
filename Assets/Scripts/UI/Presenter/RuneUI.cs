using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneUI : MonoBehaviour
{
    public static Rune SelectedRune;

    [SerializeField] private RuneInventorySO _inventory;
    [SerializeField] private UserDataSO _userData;

    [SerializeField] private RuneSlot _slotPrefab;
    [SerializeField] private Transform[] _slotHolders;

    [SerializeField] private Canvas _infoCanvas;

    [SerializeField] private RectTransform _runePanel;

    [SerializeField] private RadioGroup _mainTab;
    [SerializeField] private RadioGroup _smithyTab;

    private void Start()
    {
        _inventory.onAdded += OnRuneAdded;
        _inventory.onRemoved += OnRuneRemoved;
    }

    private void OnDestroy()
    {
        _inventory.onAdded -= OnRuneAdded;
        _inventory.onRemoved -= OnRuneRemoved;
    }

    public void OnClickEnchantButton()
    {
        SmithyUI.SelectedRuneForEnchant = SelectedRune;
        _mainTab.Select((int)MainTab.Smithy);
        _smithyTab.Select((int)SmithyTab.Enchant);
    }

    public void OnClickDisassembleButton()
    {
        SmithyUI.DisassembleState = DisassembleState.RuneSelected;
        SmithyUI.SelectedRuneForDisassemble = SelectedRune;
        _mainTab.Select((int)MainTab.Smithy);
        _smithyTab.Select((int)SmithyTab.Disassemble);
    }

    public void Sell()
    {
        _userData.EarnMoney(MoneyType.Gold, SelectedRune.Info.ResalePrice);
        _inventory.Remove(SelectedRune);
    }

    public void CloseInfoUI()
    {
        CorpsUI.State = State.Idle;
        SelectedRune = null;

        _infoCanvas.enabled = false;
    }

    private void OnClickSlot(Rune rune)
    {
        switch (CorpsUI.State)
        {
            case State.Idle:
                CorpsUI.State = State.ShowRuneInfo;
                SelectedRune = rune;
                OpenInfoUI(rune);
                break;

            case State.ClickRuneEnchantSlot:
                SmithyUI.SelectedRuneForEnchant = rune;
                Cutout.Cancel?.Invoke();
                break;

            case State.ClickDisassembleSlot:
                SmithyUI.DisassembleState = DisassembleState.RuneSelected;
                SmithyUI.SelectedRuneForDisassemble = rune;
                Cutout.Cancel?.Invoke();
                break;
        }
    }

    private void OpenInfoUI(Rune rune)
    {
        SelectedRune = rune;

        UpdateUI(rune);
        _infoCanvas.enabled = true;
    }

    private void UpdateUI(Rune rune)
    {

    }

    private void OnRuneAdded(Rune rune)
    {
        RuneSlot slot = Instantiate(_slotPrefab, _slotHolders[(int)rune.Info.Type]);
        slot.Init(() => OnClickSlot(rune));

        rune.onValueChanged += slot.UpdateUI;
        rune.onRemoved += slot.Remove;
    }

    private void OnRuneRemoved(Rune rune)
    {
        rune.onRemoved?.Invoke();
        CloseInfoUI();
    }
}
