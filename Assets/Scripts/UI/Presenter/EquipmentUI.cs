using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentUI : MonoBehaviour
{
    public static Equipment SelectedEquipment;

    [SerializeField] private EquipmentInventorySO _inventory;
    [SerializeField] private CorpsSO _corps;
    [SerializeField] private UserDataSO _userData;

    [SerializeField] private EquipmentSlot _slotPrefab;
    [SerializeField] private Transform[] _slotHolders;

    [SerializeField] private Canvas _infoCanvas;
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _starText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _enchantableCountText;
    [SerializeField] private TextMeshProUGUI _buffsText;
    [SerializeField] private TextMeshProUGUI _enchantedBuffsText;
    [SerializeField] private Button _disassembleButton;
    [SerializeField] private Button _sellButton;

    [SerializeField] private RectTransform _corpsPanel;

    [SerializeField] private RadioGroup _mainTab;
    [SerializeField] private RadioGroup _smithyTab;

    private void Start()
    {
        _inventory.onAdded += OnEquipmentAdded;
        _inventory.onRemoved += OnEquipmentRemoved;
    }

    private void OnDestroy()
    {
        _inventory.onAdded -= OnEquipmentAdded;
        _inventory.onRemoved -= OnEquipmentRemoved;
    }

    public void OnClickEquipButton()
    {
        Cutout.Do?.Invoke(_corpsPanel);
    }

    public void Release()
    {
        for (int i = 0; i < SelectedEquipment.CombatPositionIndices.Length; i++)
        {
            int combatPositionIndex = SelectedEquipment.CombatPositionIndices[i];
            if (combatPositionIndex != -1)
            {
                Combatant combatant = _corps.Presets[i].Combatants[SelectedEquipment.CombatPositionIndices[i]];
                combatant.ReleaseEquipment(SelectedEquipment.SlotIndices[i]);
                SelectedEquipment.Released(CorpsUI.SelectedPresetIndex);
            }
        }
    }

    public void OnClickUpgradeButton()
    {
        SmithyUI.UpgradeState = UpgradeState.EquipmentSelected;
        SmithyUI.SelectedEquipmentForUpgrade = SelectedEquipment;
        _mainTab.Select((int)MainTab.Smithy);
        _smithyTab.Select((int)SmithyTab.Upgrade);
    }

    public void OnClickEnchantButton()
    {
        SmithyUI.SelectedEquipmentForEnchant = SelectedEquipment;
        _mainTab.Select((int)MainTab.Smithy);
        _smithyTab.Select((int)SmithyTab.Enchant);
    }

    public void OnClickDisassembleButton()
    {
        SmithyUI.DisassembleState = DisassembleState.EquipmentSelected;
        SmithyUI.SelectedEquipmentForDisassemble = SelectedEquipment;
        _mainTab.Select((int)MainTab.Smithy);
        _smithyTab.Select((int)SmithyTab.Disassemble);
    }

    public void Sell()
    {
        _userData.EarnMoney(MoneyType.Gold, SelectedEquipment.Info.ResalePrice);
        _inventory.Remove(SelectedEquipment);
        CloseInfoUI();
    }

    public void CloseInfoUI()
    {
        CorpsUI.State = State.Idle;

        if (SelectedEquipment != null) SelectedEquipment.onValueChanged -= OnEquipmentValueChanged;
        SelectedEquipment = null;

        _infoCanvas.enabled = false;
    }

    private void OnClickSlot(Equipment equipment)
    {
        switch (CorpsUI.State)
        {
            case State.Idle:
                CorpsUI.State = State.ShowEquipmentInfo;
                SelectedEquipment = equipment;
                OpenInfoUI(equipment);
                break;

            case State.SelectEquipmentSlot:
                Equip(equipment);
                Cutout.Cancel?.Invoke();
                break;

            case State.ClickUpgradeSlot:
                SmithyUI.UpgradeState = UpgradeState.EquipmentSelected;
                SmithyUI.SelectedEquipmentForUpgrade = equipment;
                Cutout.Cancel?.Invoke();
                break;

            case State.ClickEquipmentEnchantSlot:
                SmithyUI.SelectedEquipmentForEnchant = equipment;
                Cutout.Cancel?.Invoke();
                break;

            case State.ClickDisassembleSlot:
                if (equipment.IsEquipped)
                {
                    AlertModal.Do?.Invoke("장착 중인 장비는 해체할 수 없습니다");
                }
                else
                {
                    SmithyUI.DisassembleState = DisassembleState.EquipmentSelected;
                    SmithyUI.SelectedEquipmentForDisassemble = equipment;
                    Cutout.Cancel?.Invoke();
                }
                break;
        }
    }

    private void OpenInfoUI(Equipment equipment)
    {
        equipment.onValueChanged += OnEquipmentValueChanged;
        SelectedEquipment = equipment;

        OnEquipmentValueChanged(equipment);
        _infoCanvas.enabled = true;
    }

    private void Equip(Equipment equipment)
    {
        Combatant combatant = CorpsUI.SelectedCombatant;
        int presetIndex = CorpsUI.SelectedPresetIndex;
        int combatPositionIndex = CorpsUI.SelectedCombatPositionIndex;
        int slotIndex = CorpsUI.SelectedEquipmentSlotIndex;

        int equippedCombatPositionIndex = equipment.CombatPositionIndices[presetIndex];
        int equippedSlotIndex = equipment.SlotIndices[presetIndex];

        if (equippedCombatPositionIndex != -1 && slotIndex != -1)
        {
            if (equippedCombatPositionIndex == combatPositionIndex && equippedSlotIndex == slotIndex)
                return;

            _corps.Presets[presetIndex].Combatants[combatPositionIndex].ReleaseEquipment(slotIndex);
        }
        if (combatant.IsEquipmentEquippedAt(slotIndex))
        {
            Equipment replaced = combatant.ReleaseEquipment(slotIndex);
            replaced.Released(presetIndex);
        }
        combatant.EquipEquipment(slotIndex, equipment);
        equipment.Equipped(presetIndex, combatPositionIndex, slotIndex);
    }

    private void OnEquipmentValueChanged(Equipment equipment)
    {
        _previewImage.sprite = equipment.Info.Preview;
        _starText.text = equipment.Info.Star.ToString();
        _nameText.text = $"+{equipment.Level} {equipment.Info.Name}";
        _enchantableCountText.text = $"연성 가능 횟수 {equipment.EnchantableCount}";
        _buffsText.text = equipment.BuffsDescription;
        _enchantedBuffsText.text = equipment.EnchantedBuffsDescription;

        _disassembleButton.interactable = !equipment.IsEquipped;
        _sellButton.interactable = !equipment.IsEquipped;
    }

    private void OnEquipmentAdded(Equipment equipment)
    {
        EquipmentSlot slot = Instantiate(_slotPrefab, _slotHolders[(int)equipment.Info.Type]);
        slot.Init(() => OnClickSlot(equipment));

        equipment.onValueChanged += slot.UpdateUI;
    }

    private void OnEquipmentRemoved(Equipment equipment)
    {
        Destroy(_slotHolders[(int)equipment.Info.Type].GetChild(_inventory.Items.IndexOf(equipment)).gameObject);
    }
}
