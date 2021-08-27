using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public static Skill SelectedSkill;

    [SerializeField] private SkillInventorySO _inventory;
    [SerializeField] private CorpsSO _corps;
    [SerializeField] private UserDataSO _userData;

    [SerializeField] private SkillSlot _slotPrefab;
    [SerializeField] private Transform[] _slotHolders;

    [SerializeField] private Canvas _infoCavnas;
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _starText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Button _disassembleButton;
    [SerializeField] private Button _sellButton;

    [SerializeField] private RectTransform _corpsPanel;

    [SerializeField] private RadioGroup _mainTab;
    [SerializeField] private RadioGroup _smithyTab;

    private void Start()
    {
        _inventory.onAdded += OnSkillAdded;
        _inventory.onRemoved += OnSkillRemoved;
    }

    private void OnDestroy()
    {
        _inventory.onAdded -= OnSkillAdded;
        _inventory.onRemoved -= OnSkillRemoved;
    }

    public void OnClickEquipButton()
    {
        Cutout.Do(_corpsPanel);
    }

    public void OnClickReleaseButton()
    {
        for (int i = 0; i < SelectedSkill.CombatPositionIndices.Length; i++)
        {
            int combatPositionIndex = SelectedSkill.CombatPositionIndices[i];
            if (combatPositionIndex != -1)
            {
                Combatant combatant = _corps.Presets[i].Combatants[SelectedSkill.CombatPositionIndices[i]];
                combatant.ReleaseEquipment(SelectedSkill.SlotIndices[i]);
                SelectedSkill.Released(CorpsUI.SelectedPresetIndex);
            }
        }
    }

    public void OnClickUpgradeButton()
    {
        SmithyUI.UpgradeState = UpgradeState.SkillSelected;
        SmithyUI.SelectedSkillForUpgrade = SelectedSkill;
        _mainTab.Select((int)MainTab.Smithy);
        _smithyTab.Select((int)SmithyTab.Upgrade);
    }

    public void OnClickDisassembleButton()
    {
        SmithyUI.DisassembleState = DisassembleState.SkillSelected;
        SmithyUI.SelectedSkillForUpgrade = SelectedSkill;
        _mainTab.Select((int)MainTab.Smithy);
        _smithyTab.Select((int)SmithyTab.Disassemble);
    }

    public void Sell()
    {
        _userData.EarnMoney(MoneyType.Gold, SelectedSkill.Info.ResalePrice);
        _inventory.Remove(SelectedSkill);
    }

    public void CloseInfoUI()
    {
        CorpsUI.State = State.Idle;

        if (SelectedSkill != null) SelectedSkill.onValueChanged -= OnSkillValueChanged;
        SelectedSkill = null;

        _infoCavnas.enabled = false;
    }

    private void OnClickSlot(Skill skill)
    {
        switch (CorpsUI.State)
        {
            case State.Idle:
                CorpsUI.State = State.ShowSkillInfo;
                SelectedSkill = skill;
                OpenInfoUI(skill);
                break;

            case State.SelectSkillSlot:
                Equip(skill);
                Cutout.Cancel?.Invoke();
                break;

            case State.ClickUpgradeSlot:
                SmithyUI.UpgradeState = UpgradeState.SkillSelected;
                SmithyUI.SelectedSkillForUpgrade = skill;
                Cutout.Cancel?.Invoke();
                break;

            case State.ClickDisassembleSlot:
                if (skill.IsEquipped)
                {
                    AlertModal.Do?.Invoke("장착 중인 스킬은 해체할 수 없습니다");
                }
                else
                {
                    SmithyUI.DisassembleState = DisassembleState.SkillSelected;
                    SmithyUI.SelectedSkillForDisassemble = skill;
                    Cutout.Cancel?.Invoke();
                }
                break;
        }
    }

    private void OpenInfoUI(Skill skill)
    {
        skill.onValueChanged += OnSkillValueChanged;
        SelectedSkill = skill;

        OnSkillValueChanged(skill);
        _infoCavnas.enabled = true;
    }

    private void Equip(Skill skill)
    {
        Combatant combatant = CorpsUI.SelectedCombatant;
        int presetIndex = CorpsUI.SelectedPresetIndex;
        int combatPositionIndex = CorpsUI.SelectedCombatPositionIndex;
        int slotIndex = CorpsUI.SelectedSkillSlotIndex;

        int equippedCombatPositionIndex = skill.CombatPositionIndices[presetIndex];
        int equippedSlotIndex = skill.SlotIndices[presetIndex];

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
        combatant.EquipSkill(slotIndex, skill);
        skill.Equipped(presetIndex, combatPositionIndex, slotIndex);
    }

    private void OnSkillValueChanged(Skill skill)
    {
        _previewImage.sprite = skill.Info.Preview;
        _starText.text = skill.Info.Star.ToString();
        _nameText.text = $"+{skill.Level} {skill.Info.Name}";
        _descriptionText.text = skill.Description;

        _disassembleButton.interactable = !skill.IsEquipped;
        _sellButton.interactable = !skill.IsEquipped;
    }

    private void OnSkillAdded(Skill skill)
    {
        SkillSlot slot = Instantiate(_slotPrefab, _slotHolders[0]);
        slot.Init(() => OnClickSlot(skill));

        skill.onValueChanged += slot.UpdateUI;
        skill.onRemoved += slot.Remove;
    }

    private void OnSkillRemoved(Skill skill)
    {
        skill.onRemoved?.Invoke();
        CloseInfoUI();
    }
}
