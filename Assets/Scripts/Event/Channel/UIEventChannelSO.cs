using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UIEventChannel", menuName = "ScriptableObject/Event/UIEventChannel")]
public class UIEventChannelSO : BaseEventChannelSO
{
    // Status
    public UnityAction<UserDataSO> initStatusBarUI;
    public UnityAction<string> updateNicknameUI;
    public UnityAction<string> updateStageUI;
    public UnityAction<MoneyType, int> updateMoneyUI;

    // Combatant
    public UnityAction<bool> updatePresetInfoUI;
    public UnityAction<Combatant> updateCombatantInfoUI;
    public UnityAction<Skill> showSkillInfoTooltip;
    public UnityAction hideSkillInfoTooltip;
    public UnityAction<Equipment> showEquipmentInfoTooltip;
    public UnityAction hidEquipmentInfoTooltip;
    public UnityAction<Combatant> openCorpsInfoUIWithCutout;

    // Character
    public UnityAction<List<Character>, UnityAction<Character>> initCharacterInventoryUI;
    public UnityAction<Character, UnityAction<Character>> addCharacterSlot;
    public UnityAction<int, int> removeCharacterSlot;
    public UnityAction<int, Character> updateCharacterSlot;
    public UnityAction openCharacterInfoUIWithCutout;
    public UnityAction<Character> openCharacterInfoUI;
    public UnityAction closeCharacterInfoUI;
    public UnityAction updateCharacterInfoUI;
    public UnityAction updateCombatingCharacterInfoUI;

    // Equipment
    public UnityAction<List<Equipment>, UnityAction<Equipment>> initEquipmentInventoryUI;
    public UnityAction<Equipment, UnityAction<Equipment>> addEquipmentSlot;
    public UnityAction<int, int> removeEquipmentSlot;
    public UnityAction<int, Equipment> updateEquipmentSlot;
    public UnityAction openEquipmentInventoryUIWithCutout;
    public UnityAction openInventoryUIWithCutout;
    public UnityAction<Equipment> openEquipmentInfoUI;
    public UnityAction closeEquipmentInfoUI;
    public UnityAction updateEquipmentInfoUI;

    // Skill
    public UnityAction<List<Skill>, UnityAction<Skill>> initSkillInventoryUI;
    public UnityAction<Skill, UnityAction<Skill>> addSkillSlot;
    public UnityAction<int, int> removeSkillSlot;
    public UnityAction<int, Skill> updateSkillSlot;
    public UnityAction openSkillInventoryUIWithCutout;
    public UnityAction<Skill> openSkillInfoUI;
    public UnityAction closeSkillInfoUI;
    public UnityAction updateSkillInfoUI;

    // Rune
    public UnityAction<List<Rune>, UnityAction<Rune>> initRuneInventoryUI;
    public UnityAction<Rune, UnityAction<Rune>> addRuneSlot;
    public UnityAction<int, int> removeRuneSlot;
    public UnityAction<int, Rune> updateRuneSlot;
    public UnityAction openRuneInventoryUIWithCutout;
    public UnityAction<Rune> openRuneInfoUI;
    public UnityAction closeRuneInfoUI;

    // Relic
    public UnityAction<List<RelicSO>> initRelicInventoryUI;
    public UnityAction<int, RelicSO> updateRelicSlot;

    // Smithy    
    public UnityAction<Sprite, CharacterStat[]> openIntroducedCharacterInfoUI;
    public UnityAction<Sprite, CharacterStat[]> updateIntroducedCharacterInfoUI;
    public UnityAction closeIntroducedCharacterInfoUI;
    public UnityAction<ItemSO[]> openMadeItemListUI;
    public UnityAction closeMadeItemListUI;
    public UnityAction<Equipment> updateUpgradeEquipmentInfoUI;
    public UnityAction<Skill> updateUpgradeSkillInfoUI;
    public UnityAction<Equipment> updateEnchantEquipmentInfoUI;
    public UnityAction<Rune> updateEnchantRuneInfoUI;
    public UnityAction<ItemSO> updateDisassembleInfoUI;

    // Quest
    public UnityAction<List<DailyQuest>, UnityAction<Quest>> openDailyQuestInfoUI;
    public UnityAction<List<RepeatableQuest>, UnityAction<Quest>> openRepeatableQuestInfoUI;
    public UnityAction closeQuestInfoUI;
    public UnityAction<int, Quest> updateDailyQuestSlot;
    public UnityAction<int, Quest> updateRepeatableQuestSlot;

    // WorldMap
    public UnityAction<WorldSO[], UnityAction<string>> initWorldListUI;
    public UnityAction<WorldSO[], UnityAction<string>> openWorldListUI;
    public UnityAction closeWorldListUI;
    public UnityAction<StageSO[], UnityAction<int>> openStageListUI;
}
