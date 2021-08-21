using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EquipmentInfoUI : BaseInfoUI<Equipment>
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _starText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _enchantableCountText;
    [SerializeField] private TextMeshProUGUI _buffsText;
    [SerializeField] private TextMeshProUGUI _enchantedBuffsText;
    [SerializeField] private GameObject _equipButton;
    [SerializeField] private GameObject _releaseButton;
    [SerializeField] private Button _disassembleButton;
    [SerializeField] private Button _sellButton;

    private UnityAction _updateUI;

    private void Start()
    {
        _channel.openEquipmentInfoUI += Open;
        _channel.closeEquipmentInfoUI += Close;
    }

    private void OnDestroy()
    {
        _channel.openEquipmentInfoUI -= Open;
        _channel.closeEquipmentInfoUI -= Close;
    }

    public override void Open(Equipment equipment)
    {
        base.Open(equipment);

        _updateUI = () => UpdateUI(equipment);
        _channel.updateEquipmentInfoUI += _updateUI;
    }

    public override void Close()
    {
        base.Close();
        _channel.updateEquipmentInfoUI -= _updateUI;
    }

    public override void UpdateUI(Equipment equipment)
    {
        _previewImage.sprite = equipment.Info.Preview;
        _starText.text = equipment.Info.Star.ToString();
        _nameText.text = $"+{equipment.Level} {equipment.Info.Name}";
        _enchantableCountText.text = $"연성 가능 횟수 {equipment.EnchantableCount}";
        _buffsText.text = equipment.BuffsDescription;
        _enchantedBuffsText.text = equipment.EnchantedBuffsDescription;

        _equipButton.SetActive(!equipment.IsEquipped);
        _releaseButton.SetActive(equipment.IsEquipped);

        _disassembleButton.interactable = !equipment.IsEquipped;
        _sellButton.interactable = !equipment.IsEquipped;
    }
}
