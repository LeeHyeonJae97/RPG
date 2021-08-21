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
            _enchantableCountText.text = $"���� ���� Ƚ�� {equipment.EnchantableCount}";
        }
        else
        {
            _equipmentPreviewImage.gameObject.SetActive(false);
            _enchantableCountText.text = "----";
        }

        // NOTE :
        // ����� Equipment ����� Star�� ���� ����?
    }

    public void UpdateRuneUI(Rune rune)
    {
        if (rune != null)
        {
            _runePreviewImage.sprite = rune.Info.Preview;
            _runePreviewImage.gameObject.SetActive(true);
            _successPercentText.text = $"����Ȯ�� {rune.Info.EnchantSuccessPercent}%";
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
