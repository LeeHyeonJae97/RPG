using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    SelectCharacterSlot,
    SelectEquipmentSlot,
    SelectSkillSlot,
    ShowCharacterInfo,
    ShowEquipmentInfo,
    ShowSkillInfo,
    ShowRuneInfo,
    ClickEquipmentEnchantSlot,
    ClickRuneEnchantSlot,
    ClickUpgradeSlot,
    ClickDisassembleSlot,
}

public class CorpsUI : MonoBehaviour
{
    public static int SelectedPresetIndex;
    public static int SelectedCombatPositionIndex;

    public static Combatant SelectedCombatant;

    public static int SelectedSkillSlotIndex;
    public static int SelectedEquipmentSlotIndex;

    public static State State;

    [SerializeField] private CorpsSO _corps;
    [SerializeField] private CharacterInventorySO _characterInventory;
    [SerializeField] private EquipmentInventorySO _equipmentInventory;
    [SerializeField] private SkillInventorySO _skillInventory;
    [SerializeField] private RuneInventorySO _runeInventory;

    [SerializeField] private Button _joinButton;
    [SerializeField] private Image _characterPreviewImage;
    [SerializeField] private Image[] _equipmentPreviewImages;
    [SerializeField] private Image[] _skillPreviewImages;
    [SerializeField] private TextMeshProUGUI _characterLevelText;
    [SerializeField] private TextMeshProUGUI _characterNameText;
    [SerializeField] private TextMeshProUGUI[] _statTexts;

    [SerializeField] private RectTransform _characterPanel;
    [SerializeField] private RectTransform _equipmentPanel;
    [SerializeField] private RectTransform _skillPanel;

    private CombatManager _combatManager;

    private void Start()
    {
        _combatManager = FindObjectOfType<CombatManager>();
    }

    public void JoinCombat()
    {
        if (!_corps.Joinable(SelectedPresetIndex))
        {
            AlertModal.Do?.Invoke("참가 가능한 전투원이 없습니다.");
            return;
        }

        _combatManager.CombatantJoined(SelectedPresetIndex);

        // NOTE :
        // 전투 재시작
    }

    public void SelectPreset(int index)
    {
        if (SelectedCombatant != null) SelectedCombatant.onValueChanged -= OnCombatantValueChanged;

        SelectedPresetIndex = index;
        SelectedCombatant = _corps.Presets[index].Combatants[SelectedCombatPositionIndex];
        SelectedCombatant.onValueChanged += OnCombatantValueChanged;
        OnCombatantValueChanged(SelectedCombatant);
    }

    public void SelectCombatPosition(int index)
    {
        if (SelectedCombatant != null) SelectedCombatant.onValueChanged -= OnCombatantValueChanged;

        SelectedCombatPositionIndex = index;
        SelectedCombatant = _corps.Presets[SelectedPresetIndex].Combatants[index];
        SelectedCombatant.onValueChanged += OnCombatantValueChanged;
        OnCombatantValueChanged(SelectedCombatant);
    }

    public void SelectCharacterSlot()
    {
        switch (State)
        {
            case State.Idle:
                if (SelectedCombatant.IsCharacterEquipped)
                {
                    ConfirmModal.Do?.Invoke("캐릭터를 해제하시겠습니까?", () => ReleaseCharacter());
                }
                else
                {
                    State = State.SelectCharacterSlot;
                    Cutout.Do(_characterPanel);
                    Cutout.OnCanceled += () => State = State.Idle;
                }
                break;

            case State.ShowCharacterInfo:
                EquipCharacter();
                Cutout.Cancel?.Invoke();
                break;
        }
    }

    public void SelectEquipmentSlot(int index)
    {
        switch (State)
        {
            case State.Idle:
                if (SelectedCombatant.IsEquipmentEquippedAt(index))
                {
                    ConfirmModal.Do?.Invoke("장비를 해제하시겠습니까?", () => ReleaseEquipment());
                }
                else
                {
                    State = State.SelectEquipmentSlot;
                    SelectedEquipmentSlotIndex = index;
                    Cutout.Do(_equipmentPanel);
                    Cutout.OnCanceled += () =>
                    {
                        SelectedEquipmentSlotIndex = -1;
                        State = State.Idle;
                    };
                }
                break;

            case State.ShowEquipmentInfo:
                EquipEquipment();
                Cutout.Cancel?.Invoke();
                break;
        }
    }

    public void SelectSkillSlot(int index)
    {
        switch (State)
        {
            case State.Idle:
                if (SelectedCombatant.IsSkillEquippedAt(index))
                {
                    SelectedSkillSlotIndex = index;
                    ConfirmModal.Do?.Invoke("스킬을 해제하시겠습니까?", () => ReleaseSkill());
                }
                else
                {
                    State = State.SelectSkillSlot;
                    SelectedSkillSlotIndex = index;
                    Cutout.Do(_skillPanel);
                    Cutout.OnCanceled += () =>
                    {
                        SelectedSkillSlotIndex = -1;
                        State = State.Idle;
                    };
                }
                break;

            case State.ShowSkillInfo:
                EquipSkill();
                Cutout.Cancel?.Invoke();
                break;
        }
    }

    private void EquipCharacter()
    {
        Character character = CharacterUI.SelectedCharacter;

        int combatPositionIndex = character.CombatPositionIndices[SelectedPresetIndex];

        if (combatPositionIndex != -1)
        {
            if (combatPositionIndex == SelectedCombatPositionIndex)
                return;

            _corps.Presets[SelectedPresetIndex].Combatants[combatPositionIndex].ReleaseCharacter();
        }
        if (SelectedCombatant.IsCharacterEquipped)
        {
            Character replaced = SelectedCombatant.ReleaseCharacter();
            replaced.Released(SelectedPresetIndex);
        }
        SelectedCombatant.EquipCharacter(character);
        character.Equipped(SelectedPresetIndex, SelectedCombatPositionIndex);
    }

    public void ReleaseCharacter()
    {
        if (!_corps.UnJoinable(SelectedPresetIndex, SelectedCombatPositionIndex))
        {
            AlertModal.Do?.Invoke("최소 한명은 전투에 참가하고 있어야합니다.");
            return;
        }

        Character character = SelectedCombatant.ReleaseCharacter();
        character.Released(SelectedPresetIndex);
    }

    private void EquipEquipment()
    {
        Equipment equipment = EquipmentUI.SelectedEquipment;

        int combatPositionIndex = equipment.CombatPositionIndices[SelectedPresetIndex];
        int slotIndex = equipment.SlotIndices[SelectedPresetIndex];

        if (combatPositionIndex != -1 && slotIndex != -1)
        {
            if (combatPositionIndex == SelectedCombatPositionIndex && slotIndex == SelectedEquipmentSlotIndex)
                return;

            _corps.Presets[SelectedPresetIndex].Combatants[combatPositionIndex].ReleaseEquipment(slotIndex);
        }
        if (SelectedCombatant.IsEquipmentEquippedAt(SelectedEquipmentSlotIndex))
        {
            Equipment replaced = SelectedCombatant.ReleaseEquipment(SelectedEquipmentSlotIndex);
            replaced.Released(SelectedPresetIndex);
        }
        SelectedCombatant.EquipEquipment(SelectedEquipmentSlotIndex, equipment);
        equipment.Equipped(SelectedPresetIndex, SelectedCombatPositionIndex, SelectedEquipmentSlotIndex);
    }

    private void ReleaseEquipment()
    {
        Equipment Released = SelectedCombatant.ReleaseEquipment(SelectedEquipmentSlotIndex);
        Released.Released(SelectedPresetIndex);
    }

    private void EquipSkill()
    {
        Skill skill = SkillUI.SelectedSkill;

        int combatPositionIndex = skill.CombatPositionIndices[SelectedPresetIndex];
        int slotIndex = skill.SlotIndices[SelectedPresetIndex];

        if (combatPositionIndex != -1 && slotIndex != -1)
        {
            if (combatPositionIndex == SelectedCombatPositionIndex && slotIndex == SelectedSkillSlotIndex)
                return;

            _corps.Presets[SelectedPresetIndex].Combatants[combatPositionIndex].ReleaseSkill(slotIndex);
        }
        if (SelectedCombatant.IsSkillEquippedAt(SelectedSkillSlotIndex))
        {
            Skill replaced = SelectedCombatant.ReleaseSkill(SelectedSkillSlotIndex);
            replaced.Released(SelectedPresetIndex);
        }
        SelectedCombatant.EquipSkill(SelectedSkillSlotIndex, skill);
        skill.Equipped(SelectedPresetIndex, SelectedCombatPositionIndex, SelectedSkillSlotIndex);
    }

    private void ReleaseSkill()
    {
        Skill released = SelectedCombatant.ReleaseSkill(SelectedSkillSlotIndex);
        released.Released(SelectedPresetIndex);
    }

    private void OnCombatantValueChanged(Combatant combatant)
    {
        // Character Preview, Level, Name
        if (combatant.IsCharacterEquipped)
        {
            _characterPreviewImage.sprite = combatant.Character.Preview;
            _characterLevelText.text = combatant.Character.Level.ToString();
            _characterNameText.text = combatant.Character.Name;

            // 전투 중 표시
            if (combatant.IsJoined)
                ;
        }
        else
        {
            _characterPreviewImage.sprite = null;
            _characterLevelText.text = "----";
            _characterNameText.text = "----";
        }

        // Equipment Preivew
        for (int i = 0; i < _equipmentPreviewImages.Length; i++)
            _equipmentPreviewImages[i].sprite = combatant.IsEquipmentEquippedAt(i) ? combatant.Equipments[i].Info.Preview : null;

        // Skill Preview
        for (int i = 0; i < _skillPreviewImages.Length; i++)
            _skillPreviewImages[i].sprite = combatant.IsSkillEquippedAt(i) ? combatant.Skills[i].Info.Preview : null;

        // Stat
        for (int i = 0; i < _statTexts.Length; i++)
        {
            CombatantStat stat = combatant.Stats[i];
            float baseValue = stat.BaseValue;
            float equipmentBuffValue = stat.EquipmentBuffValue;
            float skillBuffValue = stat.SkillBuffValue;
            _statTexts[i].text = $"{(StatType)i}    {baseValue + equipmentBuffValue + skillBuffValue} " +
                $"({baseValue}+{equipmentBuffValue}+{skillBuffValue})";
        }

        // Joinable
        _joinButton.interactable = _corps.Joinable(SelectedPresetIndex);
    }
}
