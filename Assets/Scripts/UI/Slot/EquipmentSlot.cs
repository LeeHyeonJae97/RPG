using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentSlot : BaseInventorySlot<Equipment>
{
    [SerializeField] private GameObject _equippedMarkingImage;

    public override void UpdateUI(Equipment equipment)
    {
        _previewImage.sprite = equipment.Info.Preview;
        _equippedMarkingImage.SetActive(equipment.IsEquipped);
    }
}