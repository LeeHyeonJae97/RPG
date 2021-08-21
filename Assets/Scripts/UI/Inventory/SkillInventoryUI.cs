using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillInventoryUI : BaseInventoryUI<Skill, SkillSlot>
{
    [SerializeField] protected IntEventChannelSO _inventoryTabEventChannel;

    private void Start()
    {
        _channel.initSkillInventoryUI += InitUI;
        _channel.addSkillSlot += AddSlot;
        _channel.removeSkillSlot += RemoveSlot;
        _channel.updateSkillSlot += UpdateSlot;
        _channel.openSkillInventoryUIWithCutout += OpenWithCutout;
    }

    private void OnDestroy()
    {
        _channel.initSkillInventoryUI -= InitUI;
        _channel.addSkillSlot -= AddSlot;
        _channel.removeSkillSlot -= RemoveSlot;
        _channel.updateSkillSlot -= UpdateSlot;
        _channel.openSkillInventoryUIWithCutout -= OpenWithCutout;
    }

    protected override void InitUI(List<Skill> skills, UnityAction<Skill> selectSkill)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            SkillSlot slot = Instantiate(_slotPrefab, _slotHolder[(int)skills[i].Info.Type]);
            int index = i;
            slot.Init(skills[index], () => selectSkill(skills[index]));
        }
    }

    protected override void AddSlot(Skill skill, UnityAction<Skill> selectSkill)
    {
        SkillSlot slot = Instantiate(_slotPrefab, _slotHolder[(int)skill.Info.Type]);
        slot.Init(skill, () => selectSkill(skill));
    }

    protected override void UpdateSlot(int index, Skill skill)
    {
        _slotHolder[(int)skill.Info.Type].GetChild(index).GetComponent<SkillSlot>().UpdateUI(skill);
    }
}
