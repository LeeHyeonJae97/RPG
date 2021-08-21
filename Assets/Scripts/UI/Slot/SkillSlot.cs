using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillSlot : BaseInventorySlot<Skill>
{
    [SerializeField] protected GameObject _equippedMarkingImage;

    public override void UpdateUI(Skill skill)
    {
        _previewImage.sprite = skill.Info.Preview;
        _equippedMarkingImage.SetActive(skill.IsEquipped);
    }
}
