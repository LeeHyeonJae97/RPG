using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotTooltip : Tooltip
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _nameText;

    public override void Show(System.Object equipment)
    {
        EquipmentSO info = ((Equipment)equipment).Info;

        _previewImage.sprite = info.Preview;
        _nameText.text = info.Name;

        SetPos();
    }
}
