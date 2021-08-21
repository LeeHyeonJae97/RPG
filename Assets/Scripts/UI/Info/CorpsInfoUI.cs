using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorpsInfoUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _uiEventchannel;
    [SerializeField] private CutoutEventChannelSO _cutoutEventChannel;

    [SerializeField] private RectTransform _panel;
    [SerializeField] private Button _joinButton;
    [SerializeField] private TextMeshProUGUI _joinButtonText;        
    [SerializeField] private Image _characterPreviewImage;
    [SerializeField] private Image[] _equipmentPreviewImages;
    [SerializeField] private Image[] _skillPreviewImages;
    [SerializeField] private TextMeshProUGUI _characterLevelText;
    [SerializeField] private TextMeshProUGUI _characterNameText;
    [SerializeField] private TextMeshProUGUI[] _statTexts;

    private void Start()
    {
        _uiEventchannel.updatePresetInfoUI += UpdatePresetInfo;
        _uiEventchannel.updateCombatantInfoUI += UpdateCombatantInfo;
        _uiEventchannel.openCorpsInfoUIWithCutout += OpenForEquip;
    }

    private void OnDestroy()
    {
        _uiEventchannel.updatePresetInfoUI -= UpdatePresetInfo;
        _uiEventchannel.updateCombatantInfoUI -= UpdateCombatantInfo;
        _uiEventchannel.openCorpsInfoUIWithCutout -= OpenForEquip;
    }

    public void UpdatePresetInfo(bool isJoined)
    {
        _joinButton.interactable = !isJoined;
        _joinButtonText.text = isJoined ? "전투중" : "출전";
    }

    public void UpdateCombatantInfo(Combatant combatant)
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
            CombatantStat stat = combatant.StatDic[Variables.StatNames[i]];
            float baseValue = stat.BaseValue;
            float equipmentBuffValue = stat.EquipmentBuffValue;
            float skillBuffValue = stat.SkillBuffValue;
            _statTexts[i].text = $"{Variables.StatNames[i]}    {baseValue + equipmentBuffValue + skillBuffValue} " +
                $"({baseValue}+{equipmentBuffValue}+{skillBuffValue})";
        }
    }

    public void OpenForEquip(Combatant combatant)
    {
        if(combatant != null) UpdateCombatantInfo(combatant);

        _cutoutEventChannel.show?.Invoke(_panel);
        _cutoutEventChannel.onHide += () => gameObject.SetActive(false);

        gameObject.SetActive(true);
    }
}
