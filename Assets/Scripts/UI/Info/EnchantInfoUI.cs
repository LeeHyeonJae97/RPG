using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantInfoUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;
    [SerializeField] private Image _equipmentPreviewImage;
    [SerializeField] private TextMeshProUGUI _enchantableCountText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Image _runePreviewImage;
    [SerializeField] private TextMeshProUGUI _successPercentText;
    [SerializeField] private TextMeshProUGUI _runeBuffsText;

    private void Start()
    {
        _channel.updateEnchantEquipmentInfoUI += UpdateEquipmentUI;
        _channel.updateEnchantRuneInfoUI += UpdateRuneUI;
    }

    private void OnDestroy()
    {
        _channel.updateEnchantEquipmentInfoUI -= UpdateEquipmentUI;
        _channel.updateEnchantRuneInfoUI -= UpdateRuneUI;
    }

    public void UpdateEquipmentUI(Equipment equipment)
    {
        if (equipment != null)
        {
            _equipmentPreviewImage.sprite = equipment.Info.Preview;
            _equipmentPreviewImage.gameObject.SetActive(true);
            _enchantableCountText.text = $"연성 가능 횟수 {equipment.EnchantableCount}";
        }
        else
        {
            _equipmentPreviewImage.gameObject.SetActive(false);
            _enchantableCountText.text = "----";
        }

        // NOTE :
        // 비용은 Equipment 장비의 Star에 따라 결정?
    }

    public void UpdateRuneUI(Rune rune)
    {
        if (rune != null)
        {
            _runePreviewImage.sprite = rune.Info.Preview;
            _runePreviewImage.gameObject.SetActive(true);
            _successPercentText.text = $"성공확률 {rune.Info.EnchantSuccessPercent}%";
            _runeBuffsText.text = rune.BuffsDescription;
        }
        else
        {
            _runePreviewImage.gameObject.SetActive(false);
            _successPercentText.text = "----";
            _runeBuffsText.text = "----";
        }
    }
}
