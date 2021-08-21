using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeInfoUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;

    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _successPercentText;
    [SerializeField] private TextMeshProUGUI _buffsText;

    private void Start()
    {
        _channel.updateUpgradeEquipmentInfoUI += UpdateEquipmentInfoUI;
        _channel.updateUpgradeSkillInfoUI += UpdateSkillInfoUI;
    }

    private void OnDestroy()
    {
        _channel.updateUpgradeEquipmentInfoUI -= UpdateEquipmentInfoUI;
        _channel.updateUpgradeSkillInfoUI -= UpdateSkillInfoUI;
    }

    public void UpdateEquipmentInfoUI(Equipment equipment)
    {
        if (equipment == null)
        {
            _previewImage.gameObject.SetActive(false);
            _successPercentText.text = "----";
            _costText.text = "----";
            _buffsText.text = "----";
        }
        else
        {
            _previewImage.sprite = equipment.Info.Preview;
            _previewImage.gameObject.SetActive(true);
            _successPercentText.text = "성공확률";
            _costText.text = "비용";
            _buffsText.text = equipment.BuffsDescriptionWithNextLevel;
        }
    }

    public void UpdateSkillInfoUI(Skill skill)
    {
        _previewImage.sprite = skill.Info.Preview;
        _successPercentText.text = "성공확률";
        _costText.text = "비용";
        _buffsText.text = skill.DescriptionWithNextLevel;
    }
}
