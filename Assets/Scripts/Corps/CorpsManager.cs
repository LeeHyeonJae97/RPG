using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CorpsManager : MonoBehaviour
{
    private enum State
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
        ClickEnchantRuneSlot,
        ClickUpgradeSlot,
        ClickDisassembleSlot,
    }

    private enum UpgradeState
    {
        None,
        EquipmentSelected,
        SkillSelected,
    }

    private enum DisassembleState
    {
        None,
        EquipmentSelected,
        SkillSelected,
        RuneSelected,
    }

    [SerializeField] private UIEventChannelSO _uiEventChannel;
    [SerializeField] private TooltipEventChannelSO _equipmentTooltipEventChannel;
    [SerializeField] private TooltipEventChannelSO _skillTooltipEventChannel;
    [SerializeField] private IntEventChannelSO _mainTabEventChannel;
    [SerializeField] private IntEventChannelSO _smithyTabEventChannel;
    [SerializeField] private StringEventChannelSO _alertModalEventChannel;
    [SerializeField] private ConfirmModalEventChannelSO _confirmModalEventChannel;
    [SerializeField] private CutoutEventChannelSO _cutoutEventChannel;
    [SerializeField] private WaitingEventChannelSO _waitingEventChannel;

    [SerializeField] private CorpsSO _corps;
    [SerializeField] private CharacterInventorySO _characterInventory;
    [SerializeField] private EquipmentInventorySO _equipmentInventory;
    [SerializeField] private SkillInventorySO _skillInventory;
    [SerializeField] private RuneInventorySO _runeInventory;
    [SerializeField] private RelicInventorySO _relicInventory;    

    [SerializeField] private CombatManager _combatManager;
    [SerializeField] private StatusManager _statusManager;

    private int _selectedPresetIndex;  
    private int _selectedCombatPositionIndex;
    private Combatant SelectedCombatant => _corps.Combatants[_selectedPresetIndex, _selectedCombatPositionIndex];

    private int _selectedSkillSlotIndex;
    private int _selectedEquipmentSlotIndex;

    private int _selectedCharacterIndex;
    private Character SelectedCharacter => _characterInventory.Items[_selectedCharacterIndex];

    private int _selectedEquipmentIndex;
    private Equipment SelectedEquipment => _equipmentInventory.Items[_selectedEquipmentIndex];

    private int _selectedSkillIndex;
    private Skill SelectedSkill => _skillInventory.Items[_selectedSkillIndex];

    private int _selectedRuneIndex;
    private Rune SelectedRune => _runeInventory.Items[_selectedRuneIndex];

    private int _selectedEquipmentForEnchantIndex = -1;
    private Equipment SelectedEquipmentForEnchant => _equipmentInventory.Items[_selectedEquipmentForEnchantIndex];

    private int _selectedRuneForEnchantIndex = -1;
    private Rune SelectedRunForEnchant => _runeInventory.Items[_selectedRuneForEnchantIndex];

    private int _selectedEquipmentForUpgradeIndex = -1;
    private Equipment SelectedEquipmentForUpgrade => _equipmentInventory.Items[_selectedEquipmentForUpgradeIndex];

    private int _selectedSkillForUpgradeIndex = -1;
    private Skill SelectedSkillForUpgrade => _skillInventory.Items[_selectedSkillForUpgradeIndex];

    private int _selectedEquipmentForDisassembleIndex = -1;
    private Equipment SelectedEquipmentForDisassemble => _equipmentInventory.Items[_selectedEquipmentForDisassembleIndex];

    private int _selectedSkillForDisassembleIndex = -1;
    private Skill SelectedSkillForDisassemble => _skillInventory.Items[_selectedSkillForDisassembleIndex];

    private int _selectedRuneForDisassembleIndex = -1;
    private Rune SelectedRuneForDisassemble => _runeInventory.Items[_selectedRuneForDisassembleIndex];

    [SerializeField] private State _state;
    [SerializeField] private UpgradeState _upgradeState;
    [SerializeField] private DisassembleState _disassembleState;

    private void Start()
    {
        _uiEventChannel.initCharacterInventoryUI?.Invoke(_characterInventory.Items, SelectCharacter);
        _uiEventChannel.initEquipmentInventoryUI?.Invoke(_equipmentInventory.Items, SelectEquipment);
        _uiEventChannel.initSkillInventoryUI?.Invoke(_skillInventory.Items, SelectSkill);
        _uiEventChannel.initRuneInventoryUI?.Invoke(_runeInventory.Items, SelectRune);
        _uiEventChannel.initRelicInventoryUI?.Invoke(_relicInventory.Items);

        JoinCombat(_corps.joinedPresetIndex);
    }

    #region Corps

    public void JoinCombat()
    {
        if (!_corps.Joinable(_selectedPresetIndex))
        {
            _alertModalEventChannel.onEventRaised?.Invoke("참가 가능한 전투원이 없습니다.");
            return;
        }

        _combatManager.Joined(_corps.GetCombatants(_selectedPresetIndex));
        _corps.joinedPresetIndex = _selectedPresetIndex;

        _uiEventChannel.updatePresetInfoUI?.Invoke(true);
        _uiEventChannel.updateCombatantInfoUI?.Invoke(SelectedCombatant);

        // NOTE :
        // 전투 재시작
    }

    public void JoinCombat(int index)
    {
        _combatManager.Joined(_corps.GetCombatants(index));
        _corps.joinedPresetIndex = index;

        if(index == _selectedPresetIndex)
        {
            _uiEventChannel.updatePresetInfoUI?.Invoke(true);
            _uiEventChannel.updateCombatantInfoUI?.Invoke(SelectedCombatant);
        }

        // NOTE :
        // 전투 재시작
    }

    public void SelectPreset(int index)
    {
        _selectedPresetIndex = index;
        _uiEventChannel.updatePresetInfoUI?.Invoke(index == _corps.joinedPresetIndex);
        _uiEventChannel.updateCombatantInfoUI?.Invoke(_corps.Combatants[index, _selectedCombatPositionIndex]);
    }

    public void SelectCombatPosition(int index)
    {
        _selectedCombatPositionIndex = index;
        _uiEventChannel.updateCombatantInfoUI?.Invoke(_corps.Combatants[_selectedPresetIndex, index]);
    }

    // 전투 탭 클릭시 호출

    public void UpdateCombatantInfoUI()
    {
        _uiEventChannel.updateCombatantInfoUI?.Invoke(SelectedCombatant);
    }

    public void SelectCharacterSlot()
    {
        switch (_state)
        {
            case State.Idle:
                if (SelectedCombatant.IsCharacterEquipped)
                {
                    _confirmModalEventChannel.show?.Invoke("캐릭터를 해제하시겠습니까?", () => ReleaseCharacter());
                }
                else
                {
                    _state = State.SelectCharacterSlot;
                    _uiEventChannel.openCharacterInfoUIWithCutout?.Invoke();
                    _cutoutEventChannel.onHide += () => _state = State.Idle;
                }
                break;

            case State.ShowCharacterInfo:
                if (EquipCharacter()) _uiEventChannel.updateCharacterInfoUI?.Invoke();
                _cutoutEventChannel.hide?.Invoke();
                break;
        }
    }

    private bool EquipCharacter()
    {
        int combatPositionIndex = SelectedCharacter.CombatPositionIndices[_selectedPresetIndex];
        if (combatPositionIndex != -1)
        {
            if (combatPositionIndex == _selectedCombatPositionIndex)
                return false;

            _corps.Combatants[_selectedPresetIndex, combatPositionIndex].ReleaseCharacter();
        }
        if (SelectedCombatant.IsCharacterEquipped)
        {
            Character replaced = SelectedCombatant.ReleaseCharacter();
            replaced.Released(_selectedPresetIndex);

            _uiEventChannel.updateCharacterSlot?.Invoke(_characterInventory.Items.IndexOf(replaced), replaced);
        }

        SelectedCombatant.EquipCharacter(SelectedCharacter);
        SelectedCharacter.Equipped(_selectedPresetIndex, _selectedCombatPositionIndex);

        _uiEventChannel.updateCharacterSlot?.Invoke(_selectedCharacterIndex, SelectedCharacter);

        return true;
    }

    public void ReleaseCharacter()
    {
        switch (_state)
        {
            case State.Idle:
                {
                    if (!_corps.UnJoinable(_selectedPresetIndex, _selectedCombatPositionIndex))
                    {
                        _alertModalEventChannel.onEventRaised?.Invoke("최소 한명은 전투에 참가하고 있어야합니다.");
                        return;
                    }

                    Combatant combatant = SelectedCombatant;
                    Character character = combatant.ReleaseCharacter();
                    character.Released(_selectedPresetIndex);

                    _uiEventChannel.updateCombatantInfoUI?.Invoke(combatant);
                    _uiEventChannel.updateCharacterSlot?.Invoke(_characterInventory.Items.IndexOf(character), character);
                    break;
                }

            case State.ShowCharacterInfo:
                {
                    Character character = SelectedCharacter;
                    for (int i = 0; i < character.CombatPositionIndices.Length; i++)
                    {
                        int combatPositionIndex = character.CombatPositionIndices[i];
                        if (combatPositionIndex != -1)
                        {
                            if (!_corps.UnJoinable(i, combatPositionIndex))
                            {
                                _alertModalEventChannel.onEventRaised?.Invoke("최소 한명은 전투에 참가하고 있어야합니다.");
                            }
                            else
                            {
                                Combatant combatant = _corps.Combatants[i, character.CombatPositionIndices[i]];
                                combatant.ReleaseCharacter();
                                character.Released(i);
                            }
                        }
                    }

                    _uiEventChannel.updateCharacterSlot?.Invoke(_selectedCharacterIndex, character);
                    break;
                }
        }
    }

    public void SelectEquipmentSlot(int index)
    {
        switch (_state)
        {
            case State.Idle:
                if (SelectedCombatant.IsEquipmentEquippedAt(index))
                {
                    _selectedEquipmentSlotIndex = index;
                    _confirmModalEventChannel.show?.Invoke("장비를 해제하시겠습니까?", () => ReleaseEquipment());
                }
                else
                {
                    _state = State.SelectEquipmentSlot;
                    _selectedEquipmentSlotIndex = index;

                    _uiEventChannel.openEquipmentInventoryUIWithCutout?.Invoke();
                    _cutoutEventChannel.onHide += () =>
                    {
                        _selectedEquipmentSlotIndex = -1;
                        _state = State.Idle;
                    };
                }
                break;

            case State.ShowEquipmentInfo:
                _selectedEquipmentSlotIndex = index;
                if (EquipEquipment()) _uiEventChannel.updateEquipmentInfoUI?.Invoke();
                _cutoutEventChannel.hide?.Invoke();
                break;
        }
    }

    public void LongPressEquipmentlSlot(int index)
    {
        Combatant combatant = SelectedCombatant;
        if (combatant.IsEquipmentEquippedAt(index))
            _equipmentTooltipEventChannel.show?.Invoke(combatant.Equipments[index]);
    }

    public void LongPressUpEquipmentSlot()
    {
        _equipmentTooltipEventChannel.hide?.Invoke();
    }

    public bool EquipEquipment()
    {
        int combatPositionIndex = SelectedEquipment.CombatPositionIndices[_selectedPresetIndex];
        int slotIndex = SelectedEquipment.SlotIndices[_selectedPresetIndex];

        if (combatPositionIndex != -1 && slotIndex != -1)
        {
            if (combatPositionIndex == _selectedCombatPositionIndex && slotIndex == _selectedEquipmentSlotIndex)
                return false;

            _corps.Combatants[_selectedPresetIndex, combatPositionIndex].ReleaseEquipment(slotIndex);
        }
        if (SelectedCombatant.IsEquipmentEquippedAt(_selectedEquipmentSlotIndex))
        {
            Equipment replaced = SelectedCombatant.ReleaseEquipment(_selectedEquipmentSlotIndex);
            replaced.Released(_selectedPresetIndex);

            _uiEventChannel.updateEquipmentSlot?.Invoke(_equipmentInventory.Items.IndexOf(replaced), replaced);
        }
        SelectedCombatant.EquipEquipment(_selectedEquipmentSlotIndex, SelectedEquipment);
        SelectedEquipment.Equipped(_selectedPresetIndex, _selectedCombatPositionIndex, _selectedEquipmentSlotIndex);

        _uiEventChannel.updateEquipmentSlot?.Invoke(_selectedEquipmentIndex, SelectedEquipment);

        return true;
    }

    public void ReleaseEquipment()
    {
        switch (_state)
        {
            case State.Idle:
                {
                    Combatant combatant = SelectedCombatant;
                    Equipment equipment = combatant.ReleaseEquipment(_selectedEquipmentSlotIndex);
                    equipment.Released(_selectedPresetIndex);

                    _uiEventChannel.updateCombatantInfoUI?.Invoke(combatant);
                    _uiEventChannel.updateEquipmentSlot?.Invoke(_equipmentInventory.Items.IndexOf(equipment), equipment);
                    break;
                }

            case State.ShowEquipmentInfo:
                {
                    Equipment equipment = SelectedEquipment;
                    for (int i = 0; i < equipment.CombatPositionIndices.Length; i++)
                    {
                        int combatPositionIndex = equipment.CombatPositionIndices[i];
                        if (combatPositionIndex != -1)
                        {
                            Combatant combatant = _corps.Combatants[i, equipment.CombatPositionIndices[i]];
                            combatant.ReleaseEquipment(equipment.SlotIndices[i]);
                            equipment.Released(_selectedPresetIndex);
                        }
                    }

                    _uiEventChannel.updateEquipmentSlot?.Invoke(_selectedEquipmentIndex, equipment);
                    break;
                }
        }
    }

    public void SelectSkillSlot(int index)
    {
        switch (_state)
        {
            case State.Idle:
                if (SelectedCombatant.IsSkillEquippedAt(index))
                {
                    _selectedSkillSlotIndex = index;
                    _confirmModalEventChannel.show?.Invoke("스킬을 해제하시겠습니까?", () => ReleaseSkill());
                }
                else
                {
                    _state = State.SelectSkillSlot;
                    _selectedSkillSlotIndex = index;

                    _uiEventChannel.openSkillInventoryUIWithCutout?.Invoke();
                    _cutoutEventChannel.onHide += () =>
                    {
                        _selectedSkillSlotIndex = -1;
                        _state = State.Idle;
                    };
                }
                break;

            case State.ShowSkillInfo:
                _selectedSkillSlotIndex = index;
                if (EquipSkill()) _uiEventChannel.updateSkillInfoUI?.Invoke();
                _cutoutEventChannel.hide?.Invoke();
                break;
        }
    }

    public void LongPressSkillSlot(int index)
    {
        Combatant combatant = SelectedCombatant;
        if (combatant.IsSkillEquippedAt(index))
            _skillTooltipEventChannel.show?.Invoke(combatant.Skills[index]);
    }

    public void LongPressUpSkillSlot()
    {
        _skillTooltipEventChannel.hide?.Invoke();
    }

    public bool EquipSkill()
    {
        int combatPositionIndex = SelectedSkill.CombatPositionIndices[_selectedPresetIndex];
        int slotIndex = SelectedSkill.SlotIndices[_selectedPresetIndex];

        if (combatPositionIndex != -1 && slotIndex != -1)
        {
            if (combatPositionIndex == _selectedCombatPositionIndex && slotIndex == _selectedSkillSlotIndex)
                return false;

            _corps.Combatants[_selectedPresetIndex, combatPositionIndex].ReleaseSkill(slotIndex);
        }
        if (SelectedCombatant.IsSkillEquippedAt(_selectedSkillSlotIndex))
        {
            Skill replaced = SelectedCombatant.ReleaseSkill(_selectedSkillSlotIndex);
            replaced.Released(_selectedPresetIndex);

            _uiEventChannel.updateSkillSlot?.Invoke(_skillInventory.Items.IndexOf(replaced), replaced);
        }
        SelectedCombatant.EquipSkill(_selectedSkillSlotIndex, SelectedSkill);
        SelectedSkill.Equipped(_selectedPresetIndex, _selectedCombatPositionIndex, _selectedSkillSlotIndex);

        _uiEventChannel.updateSkillSlot?.Invoke(_selectedSkillIndex, SelectedSkill);

        return true;
    }

    public void ReleaseSkill()
    {
        switch (_state)
        {
            case State.Idle:
                {
                    Combatant combatant = SelectedCombatant;
                    Skill skill = combatant.ReleaseSkill(_selectedSkillSlotIndex);
                    skill.Released(_selectedPresetIndex);

                    _uiEventChannel.updateCombatantInfoUI?.Invoke(combatant);
                    _uiEventChannel.updateSkillSlot?.Invoke(_skillInventory.Items.IndexOf(skill), skill);
                    break;
                }

            case State.ShowSkillInfo:
                {
                    Skill skill = SelectedSkill;
                    for (int i = 0; i < skill.CombatPositionIndices.Length; i++)
                    {
                        int combatPositionIndex = skill.CombatPositionIndices[i];
                        if (combatPositionIndex != -1)
                        {
                            Combatant combatant = _corps.Combatants[i, skill.CombatPositionIndices[i]];
                            combatant.ReleaseSkill(skill.SlotIndices[i]);
                            skill.Released(_selectedPresetIndex);
                        }
                    }

                    _uiEventChannel.updateSkillSlot?.Invoke(_selectedSkillIndex, skill);
                    break;
                }
        }
    }

    #endregion

    #region Character

    public void PickupCharacter(Character character)
    {
        _characterInventory.Items.Add(character);

        _uiEventChannel.addCharacterSlot?.Invoke(character, SelectCharacter);
        _uiEventChannel.closeIntroducedCharacterInfoUI?.Invoke();
    }

    public void RemoveCharacter()
    {
        // NOTE :
        // 실제로 GC에 의해서 메모리에서 내려가는지 확인 필요

        _characterInventory.Items.RemoveAt(_selectedCharacterIndex);
        _uiEventChannel.removeCharacterSlot?.Invoke(0, _selectedCharacterIndex);
        DeselectCharacter();
    }

    public void SelectCharacter(Character character)
    {
        switch (_state)
        {
            case State.Idle:
                _state = State.ShowCharacterInfo;
                _selectedCharacterIndex = _characterInventory.Items.IndexOf(character);
                _uiEventChannel.openCharacterInfoUI?.Invoke(character);
                break;

            case State.SelectCharacterSlot:
                _selectedCharacterIndex = _characterInventory.Items.IndexOf(character);
                EquipCharacter();
                _uiEventChannel.updateCombatantInfoUI?.Invoke(SelectedCombatant);
                _cutoutEventChannel.hide?.Invoke();
                break;
        }
    }

    public void DeselectCharacter()
    {
        _state = State.Idle;
        _selectedCharacterIndex = -1;
        _uiEventChannel.closeCharacterInfoUI?.Invoke();
    }

    public void TryEquipCharacter()
    {
        bool update = SelectedCharacter.CombatPositionIndices[_selectedPresetIndex] == _selectedCombatPositionIndex;
        _uiEventChannel.openCorpsInfoUIWithCutout?.Invoke(update ? SelectedCombatant : null);
    }

    public void InvestStatPoint(int index)
    {
        Character character = SelectedCharacter;
        if (character.StatPoint > 0)
        {
            character.InvestStatPoint((StatType)index);
            _uiEventChannel.updateCharacterInfoUI?.Invoke();
        }
    }

    #endregion

    #region Equipment

    public void PickupEquipment(EquipmentSO info)
    {
        Equipment equipment = new Equipment(info);
        _equipmentInventory.Items.Add(equipment);
        _uiEventChannel.addEquipmentSlot?.Invoke(equipment, SelectEquipment);
    }

    private void RemoveEquipment(int index)
    {
        int typeIndex = (int)_equipmentInventory.Items[index].Info.Type;

        _equipmentInventory.Items.RemoveAt(index);
        _uiEventChannel.removeEquipmentSlot?.Invoke(typeIndex, index);
    }

    public void SelectEquipment(Equipment equipment)
    {
        switch (_state)
        {
            case State.Idle:
                _state = State.ShowEquipmentInfo;
                _selectedEquipmentIndex = _equipmentInventory.Items.IndexOf(equipment);
                _uiEventChannel.openEquipmentInfoUI?.Invoke(equipment);
                break;

            case State.SelectEquipmentSlot:
                _selectedEquipmentIndex = _equipmentInventory.Items.IndexOf(equipment);
                EquipEquipment();
                _uiEventChannel.updateCombatantInfoUI?.Invoke(SelectedCombatant);
                _cutoutEventChannel.hide?.Invoke();
                break;

            case State.ClickUpgradeSlot:
                _upgradeState = UpgradeState.EquipmentSelected;
                _selectedEquipmentForUpgradeIndex = _equipmentInventory.Items.IndexOf(equipment);
                _uiEventChannel.updateUpgradeEquipmentInfoUI?.Invoke(equipment);
                _cutoutEventChannel.hide?.Invoke();
                break;

            case State.ClickEquipmentEnchantSlot:
                _selectedEquipmentForEnchantIndex = _equipmentInventory.Items.IndexOf(equipment);
                _uiEventChannel.updateEnchantEquipmentInfoUI?.Invoke(equipment);
                _cutoutEventChannel.hide?.Invoke();
                break;

            case State.ClickDisassembleSlot:
                if (equipment.IsEquipped)
                {
                    _alertModalEventChannel.onEventRaised?.Invoke("장착 중인 장비는 해체할 수 없습니다");
                }
                else
                {
                    _disassembleState = DisassembleState.EquipmentSelected;
                    _selectedEquipmentForDisassembleIndex = _equipmentInventory.Items.IndexOf(equipment);
                    _uiEventChannel.updateDisassembleInfoUI(equipment.Info);
                    _cutoutEventChannel.hide?.Invoke();
                }
                break;
        }
    }

    public void DeselectEquipment()
    {
        _state = State.Idle;
        _selectedEquipmentIndex = -1;
        _uiEventChannel.closeEquipmentInfoUI?.Invoke();
    }

    public void TryEquipEquipment()
    {
        _uiEventChannel.openCorpsInfoUIWithCutout?.Invoke(SelectedCombatant);
    }

    public void SelectEquipmentForUpgrade()
    {
        _selectedEquipmentForUpgradeIndex = _selectedEquipmentIndex;
        _mainTabEventChannel.onEventRaised?.Invoke((int)MainTab.Smithy);
        _smithyTabEventChannel.onEventRaised?.Invoke((int)SmithyTab.Upgrade);
        _uiEventChannel.updateUpgradeEquipmentInfoUI?.Invoke(SelectedEquipment);
    }

    public void SelectEquipmentForEnchant()
    {
        _selectedEquipmentForEnchantIndex = _selectedEquipmentIndex;
        _mainTabEventChannel.onEventRaised?.Invoke((int)MainTab.Smithy);
        _smithyTabEventChannel.onEventRaised?.Invoke((int)SmithyTab.Enchant);
        _uiEventChannel.updateEnchantEquipmentInfoUI(SelectedEquipment);
    }

    public void SelectEquipmentForDisassemble()
    {
        _disassembleState = DisassembleState.EquipmentSelected;
        _selectedEquipmentForDisassembleIndex = _selectedEquipmentIndex;
        _mainTabEventChannel.onEventRaised?.Invoke((int)MainTab.Smithy);
        _smithyTabEventChannel.onEventRaised?.Invoke((int)SmithyTab.Disassemble);
        _uiEventChannel.updateDisassembleInfoUI(SelectedEquipment.Info);
    }

    public void SellEquipment()
    {        
        _statusManager.EarnMoney(MoneyType.Gold, SelectedEquipment.Info.ResalePrice);
        RemoveEquipment(_selectedEquipmentIndex);
        DeselectEquipment();
    }

    #endregion

    #region Skill

    public void PickupSkill(SkillSO info)
    {
        Skill skill = new Skill(info);
        _skillInventory.Items.Add(skill);
        _uiEventChannel.addSkillSlot?.Invoke(skill, SelectSkill);
    }

    public void RemoveSkill(int index)
    {
        // NOTE :
        // 실제로 GC에 의해서 메모리에서 내려가는지 확인 필요

        int typeIndex = (int)_skillInventory.Items[index].Info.Type;

        _skillInventory.Items.RemoveAt(index);
        _uiEventChannel.removeSkillSlot(typeIndex, index);
    }

    public void SelectSkill(Skill skill)
    {
        switch (_state)
        {
            case State.Idle:
                _state = State.ShowSkillInfo;
                _selectedSkillIndex = _skillInventory.Items.IndexOf(skill);
                _uiEventChannel.openSkillInfoUI?.Invoke(skill);
                break;

            case State.SelectSkillSlot:
                _selectedSkillIndex = _skillInventory.Items.IndexOf(skill);
                EquipSkill();
                _uiEventChannel.updateCombatantInfoUI?.Invoke(SelectedCombatant);
                _cutoutEventChannel.hide?.Invoke();
                break;

            case State.ClickUpgradeSlot:
                _upgradeState = UpgradeState.SkillSelected;
                _selectedSkillForUpgradeIndex = _skillInventory.Items.IndexOf(skill);
                _uiEventChannel.updateUpgradeSkillInfoUI?.Invoke(skill);
                _cutoutEventChannel.hide?.Invoke();
                break;

            case State.ClickDisassembleSlot:
                if (skill.IsEquipped)
                {
                    _alertModalEventChannel.onEventRaised?.Invoke("장착 중인 스킬은 해체할 수 없습니다");
                }
                else
                {
                    _disassembleState = DisassembleState.SkillSelected;
                    _selectedSkillForDisassembleIndex = _skillInventory.Items.IndexOf(skill);
                    _uiEventChannel.updateDisassembleInfoUI?.Invoke(skill.Info);
                    _cutoutEventChannel.hide?.Invoke();
                }
                break;
        }
    }

    public void DeselectSkill()
    {
        _state = State.Idle;
        _selectedSkillIndex = -1;
        _uiEventChannel.closeSkillInfoUI?.Invoke();
    }

    public void TryEquipSkill()
    {       
        _uiEventChannel.openCorpsInfoUIWithCutout?.Invoke(SelectedCombatant);
    }

    public void SelectSkillForUpgrade()
    {
        _selectedSkillForUpgradeIndex = _selectedSkillIndex;
        _mainTabEventChannel.onEventRaised?.Invoke((int)MainTab.Smithy);
        _smithyTabEventChannel.onEventRaised?.Invoke((int)SmithyTab.Upgrade);
        _uiEventChannel.updateUpgradeSkillInfoUI?.Invoke(SelectedSkill);
    }

    public void SelectSkillForDisassemble()
    {
        _disassembleState = DisassembleState.SkillSelected;
        _selectedSkillForDisassembleIndex = _selectedSkillIndex;
        _mainTabEventChannel.onEventRaised?.Invoke((int)MainTab.Smithy);
        _smithyTabEventChannel.onEventRaised?.Invoke((int)SmithyTab.Disassemble);
        _uiEventChannel.updateDisassembleInfoUI?.Invoke(_skillInventory.Items[_selectedSkillIndex].Info);
    }

    public void SellSkill()
    {
        Skill skill = SelectedSkillForDisassemble;
        _statusManager.EarnMoney(MoneyType.Gold, skill.Info.ResalePrice);
        RemoveSkill(_selectedSkillIndex);
    }

    #endregion

    #region Rune

    public void PickupRune(RuneSO info)
    {
        Rune rune = new Rune(info);
        _runeInventory.Items.Add(rune);
        _uiEventChannel.addRuneSlot?.Invoke(rune, SelectRune);
    }

    private void RemoveRune(int index)
    {
        // NOTE :
        // 실제로 GC에 의해서 메모리에서 내려가는지 확인 필요

        int typeIndex = (int)_runeInventory.Items[index].Info.Type;

        _runeInventory.Items.RemoveAt(index);
        _uiEventChannel.removeRuneSlot(typeIndex, index);
    }

    public void SelectRune(Rune rune)
    {
        switch (_state)
        {
            case State.Idle:
                _state = State.ShowRuneInfo;
                _selectedRuneIndex = _runeInventory.Items.IndexOf(rune);
                _uiEventChannel.openRuneInfoUI?.Invoke(rune);
                break;

            case State.ClickUpgradeSlot:
                _alertModalEventChannel.onEventRaised?.Invoke("룬은 강화할 수 없습니다.");
                break;

            case State.ClickEnchantRuneSlot:
                _selectedRuneForEnchantIndex = _runeInventory.Items.IndexOf(rune);
                _uiEventChannel.updateEnchantRuneInfoUI?.Invoke(rune);
                _cutoutEventChannel.hide?.Invoke();
                break;

            case State.ClickDisassembleSlot:
                _disassembleState = DisassembleState.RuneSelected;
                _selectedRuneForDisassembleIndex = _runeInventory.Items.IndexOf(rune);
                _uiEventChannel.updateDisassembleInfoUI(rune.Info);
                _cutoutEventChannel.hide?.Invoke();
                break;
        }
    }

    public void DeselectRune()
    {
        _state = State.Idle;
        _selectedRuneIndex = -1;
        _uiEventChannel.closeRuneInfoUI?.Invoke();
    }

    public void SelectRuneForEnchant()
    {
        _selectedRuneForEnchantIndex = _selectedRuneIndex;
        _mainTabEventChannel.onEventRaised?.Invoke((int)MainTab.Smithy);
        _smithyTabEventChannel.onEventRaised?.Invoke((int)SmithyTab.Enchant);
        _uiEventChannel.updateEnchantRuneInfoUI?.Invoke(SelectedRunForEnchant);
    }

    public void SelectRuneForDisassemble()
    {
        _disassembleState = DisassembleState.RuneSelected;
        _selectedRuneForDisassembleIndex = _selectedRuneIndex;
        _mainTabEventChannel.onEventRaised?.Invoke((int)MainTab.Smithy);
        _smithyTabEventChannel.onEventRaised?.Invoke((int)SmithyTab.Disassemble);
        _uiEventChannel.updateDisassembleInfoUI(SelectedRune.Info);
    }

    public void SellRune()
    {
        Rune rune = SelectedRuneForDisassemble;
        _statusManager.EarnMoney(MoneyType.Gold, rune.Info.ResalePrice);
        RemoveRune(_selectedRuneIndex);
    }

    #endregion

    #region Smithy

    #region Make

    public string CharacterName { private get; set; }
    private CharacterStat[] _randomStats;
    private AsyncOperationHandle<IList<Sprite>> _characterPreviewLoadHandle;
    private int _characterPreviewIndex;

    public void IntroduceCharacter()
    {
        StartCoroutine(IntroduceCharacterCoroutine());
    }

    public IEnumerator IntroduceCharacterCoroutine()
    {
        if (!_statusManager.Affordable(MoneyType.Diamond, Variables.COST_INTRODUCE_CHARACTER)) yield break;

        _statusManager.LoseMoney(MoneyType.Diamond, Variables.COST_INTRODUCE_CHARACTER);

        // 캐릭터 외형
        _characterPreviewLoadHandle = Addressables.LoadAssetsAsync<Sprite>("CharacterPreview", null);
        yield return new WaitUntil(() => _characterPreviewLoadHandle.IsDone);

        _characterPreviewIndex = 0;        

        // NOTE :
        // 값의 랜덤 범위가 스탯별로 달라야한다
        _randomStats = new CharacterStat[Variables.STAT_TYPE_COUNT];
        for (int i = 0; i < _randomStats.Length; i++)
            _randomStats[i] = new CharacterStat(Random.Range(5, 11));

        _uiEventChannel.openIntroducedCharacterInfoUI?.Invoke(_characterPreviewLoadHandle.Result[_characterPreviewIndex], _randomStats);
    }

    public void Reintroduce()
    {
        for (int i = 0; i < _randomStats.Length; i++)
            _randomStats[i].Value = Random.Range(3, 10);

        _uiEventChannel.updateIntroducedCharacterInfoUI?.Invoke(_characterPreviewLoadHandle.Result[_characterPreviewIndex], _randomStats);
    }

    public void SwitchCharacterPreview(bool left)
    {
        _characterPreviewIndex = left ? _characterPreviewIndex - 1 : _characterPreviewIndex + 1;
        _characterPreviewIndex = Mathf.Clamp(_characterPreviewIndex, 0, _characterPreviewLoadHandle.Result.Count - 1);

        _uiEventChannel.updateIntroducedCharacterInfoUI?.Invoke(_characterPreviewLoadHandle.Result[_characterPreviewIndex], _randomStats);
    }

    public void Hire()
    {
        Character character = new Character(CharacterName, _characterPreviewLoadHandle.Result[_characterPreviewIndex], _randomStats);
        PickupCharacter(character);

        Addressables.Release(_characterPreviewLoadHandle);
    }

    public void MakeEquipments(int count)
    {
        if (!_statusManager.Affordable(MoneyType.Diamond, Variables.COST_MAKE_EQUIPMENT)) return;

        _statusManager.LoseMoney(MoneyType.Diamond, Variables.COST_MAKE_EQUIPMENT);
        StartCoroutine(MakeEquipmentsCoroutine(count));
    }

    public void MakeSkills(int count)
    {
        if (!_statusManager.Affordable(MoneyType.Diamond, Variables.COST_MAKE_SKILL)) return;

        _statusManager.LoseMoney(MoneyType.Diamond, Variables.COST_MAKE_SKILL);
        StartCoroutine(MakeSkillsCoroutine(count));
    }

    public void MakeRunes(int count)
    {
        if (!_statusManager.Affordable(MoneyType.Diamond, Variables.COST_MAKE_RUNE)) return;

        _statusManager.LoseMoney(MoneyType.Diamond, Variables.COST_MAKE_RUNE);
        StartCoroutine(MakeRunesCoroutine(count));
    }

    public IEnumerator MakeEquipmentsCoroutine(int count)
    {
        _waitingEventChannel.begin?.Invoke("장비 제작...");

        AsyncOperationHandle<IList<EquipmentSO>> handle = Addressables.LoadAssetsAsync<EquipmentSO>("Equipments", null);
        yield return new WaitUntil(() => handle.IsDone);

        List<EquipmentSO> infos = handle.Result.ToList();

        // NOTE :
        // 확률에 따라 랜덤으로 count 만큼 선정하도록 수정
        EquipmentSO[] picks = new EquipmentSO[count];
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, infos.Count);
            AsyncOperationHandle<EquipmentSO> handle2 = Addressables.LoadAssetAsync<EquipmentSO>($"Equipment_{infos[index].Id}");
            yield return new WaitUntil(() => handle2.IsDone);

            picks[i] = handle2.Result;
            PickupEquipment(picks[i]);
        }
        Addressables.Release(handle);

        _waitingEventChannel.end?.Invoke();

        // 선정된 Equipment를 UI에 표시하고 Inventory에 추가
        _uiEventChannel.openMadeItemListUI?.Invoke(picks);
    }

    private IEnumerator MakeSkillsCoroutine(int count)
    {
        _waitingEventChannel.begin?.Invoke("");

        AsyncOperationHandle<IList<SkillSO>> handle = Addressables.LoadAssetsAsync<SkillSO>("Skills", null);
        yield return new WaitUntil(() => handle.IsDone);

        List<SkillSO> infos = handle.Result.ToList();

        // NOTE :        
        // 확률에 따라 랜덤으로 count만큼 선정하도록 수정
        SkillSO[] picks = new SkillSO[count];
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, infos.Count);
            AsyncOperationHandle<SkillSO> handle2 = Addressables.LoadAssetAsync<SkillSO>($"Skill_{infos[index].Id}");
            yield return new WaitUntil(() => handle2.IsDone);

            picks[i] = handle2.Result;
            PickupSkill(picks[i]);
        }
        Addressables.Release(handle);

        _waitingEventChannel.end?.Invoke();

        // 선정된 Equipment를 UI에 표시하고 Inventory에 추가
        _uiEventChannel.openMadeItemListUI?.Invoke(picks);
    }

    private IEnumerator MakeRunesCoroutine(int count)
    {
        _waitingEventChannel.begin?.Invoke("");

        AsyncOperationHandle<IList<RuneSO>> handle = Addressables.LoadAssetsAsync<RuneSO>("Runes", null);
        yield return new WaitUntil(() => handle.IsDone);

        List<RuneSO> infos = handle.Result.ToList();

        RuneSO[] picks = new RuneSO[count];
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, infos.Count);
            AsyncOperationHandle<RuneSO> handle2 = Addressables.LoadAssetAsync<RuneSO>($"Rune_{infos[index].Id}");
            yield return new WaitUntil(() => handle2.IsDone);

            picks[i] = handle2.Result;
            PickupRune(picks[i]);
        }
        Addressables.Release(handle);

        _waitingEventChannel.end?.Invoke();

        // 선정된 Equipment를 UI에 표시하고 Inventory에 추가
        _uiEventChannel.openMadeItemListUI?.Invoke(picks);
    }

    #endregion

    #region Upgrade

    public void ClickUpgradeSlot()
    {
        if (_upgradeState != UpgradeState.None)
        {
            _upgradeState = UpgradeState.None;
            _uiEventChannel.updateUpgradeEquipmentInfoUI?.Invoke(null);
        }
        else
        {
            _state = State.ClickUpgradeSlot;
            _uiEventChannel.openInventoryUIWithCutout?.Invoke();
            _cutoutEventChannel.onHide += () => _state = State.Idle;
        }
    }

    public void Upgrade()
    {
        if (_upgradeState == UpgradeState.None)
        {
            _alertModalEventChannel.onEventRaised?.Invoke("강화할 장비/스킬을 선택해 주세요");
            return;
        }
        else
        {
            if (!_statusManager.Affordable(MoneyType.ForUpgrade, Variables.COST_UPGRADE)) return;

            _statusManager.LoseMoney(MoneyType.ForUpgrade, Variables.COST_UPGRADE);

            bool success;

            switch (_upgradeState)
            {
                case UpgradeState.EquipmentSelected:
                    success = SelectedEquipmentForUpgrade.Upgraded();
                    break;

                case UpgradeState.SkillSelected:
                    success = SelectedSkillForUpgrade.Upgraded();
                    break;

                default:
                    Debug.LogError("Error");
                    return;
            }
            _alertModalEventChannel.onEventRaised?.Invoke(success ? "강화 성공!" : "강화 실패!");
        }
    }

    #endregion

    #region Enchant

    public void ClickEnchantEquipmentSlot()
    {
        if (_selectedEquipmentForEnchantIndex != -1)
        {
            _selectedEquipmentForEnchantIndex = -1;
            _uiEventChannel.updateEnchantEquipmentInfoUI?.Invoke(null);
        }
        else
        {
            _state = State.ClickEquipmentEnchantSlot;
            _uiEventChannel.openEquipmentInventoryUIWithCutout?.Invoke();
            _cutoutEventChannel.onHide += () => _state = State.Idle;
        }
    }

    public void ClickEnchantRuneSlot()
    {
        if (_selectedRuneForEnchantIndex != -1)
        {
            _selectedRuneForEnchantIndex = -1;
            _uiEventChannel.updateEnchantRuneInfoUI?.Invoke(null);
        }
        else
        {
            _state = State.ClickEnchantRuneSlot;
            _uiEventChannel.openRuneInventoryUIWithCutout?.Invoke();
            _cutoutEventChannel.onHide += () => _state = State.Idle;
        }
    }

    public void Enchant()
    {
        if (_selectedEquipmentForEnchantIndex == -1)
        {
            _alertModalEventChannel.onEventRaised?.Invoke("장비를 선택해주세요");
            return;
        }
        if (_selectedRuneForEnchantIndex == -1)
        {
            _alertModalEventChannel.onEventRaised?.Invoke("룬을 선택해주세요");
            return;
        }
        if (!_statusManager.Affordable(MoneyType.ForEnchant, Variables.COST_ENCHANT)) return;

        Equipment equipment = SelectedEquipmentForEnchant;
        if (!equipment.Enchantable)
        {
            _alertModalEventChannel.onEventRaised?.Invoke("더 이상 연성이 불가능합니다.");
            return;
        }

        _statusManager.LoseMoney(MoneyType.ForEnchant, Variables.COST_ENCHANT);

        Rune rune = SelectedRunForEnchant;

        bool success = equipment.Enchanted(rune);
        _alertModalEventChannel.onEventRaised?.Invoke(success ? "연성 성공!" : "연성 실패!");

        RemoveRune(_selectedRuneForEnchantIndex);
        _selectedRuneForEnchantIndex = -1;
        _uiEventChannel.updateEnchantRuneInfoUI?.Invoke(null);
    }

    #endregion

    #region Disassemble

    public void ClickDisassembleSlot()
    {
        if (_disassembleState != DisassembleState.None)
        {
            _disassembleState = DisassembleState.None;
            _uiEventChannel.updateDisassembleInfoUI?.Invoke(null);
        }
        else
        {
            _state = State.ClickDisassembleSlot;
            _uiEventChannel.openInventoryUIWithCutout?.Invoke();
            _cutoutEventChannel.onHide += () => _state = State.Idle;
        }
    }

    public void Disassemble()
    {
        if (_disassembleState == DisassembleState.None)
        {
            _alertModalEventChannel.onEventRaised?.Invoke("분해할 장비/스킬/룬을(를) 선택해 주세요");
            return;
        }
        else
        {
            if (!_statusManager.Affordable(MoneyType.Gold, Variables.COST_DISASSEMBLE)) return;

            _statusManager.LoseMoney(MoneyType.Gold, Variables.COST_DISASSEMBLE);

            int amount;

            _disassembleState = DisassembleState.None;
            switch (_disassembleState)
            {
                case DisassembleState.EquipmentSelected:
                    amount = SelectedEquipmentForDisassemble.Disassembled();
                    RemoveEquipment(_selectedEquipmentForDisassembleIndex);
                    break;

                case DisassembleState.SkillSelected:
                    amount = SelectedSkillForDisassemble.Disassembled();
                    RemoveSkill(_selectedSkillForDisassembleIndex);
                    break;

                case DisassembleState.RuneSelected:
                    amount = SelectedRuneForDisassemble.Disassembled();
                    RemoveRune(_selectedRuneForDisassembleIndex);
                    break;

                default:
                    Debug.LogError("Error");
                    return;
            }
            _statusManager.EarnMoney(MoneyType.ForUpgrade, amount);
            _uiEventChannel.updateDisassembleInfoUI?.Invoke(null);
        }
    }

    #endregion

    #endregion
}
