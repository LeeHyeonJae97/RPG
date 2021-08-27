using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Combatant
{
    public Character Character { get; private set; }
    public Equipment[] Equipments { get; private set; } = new Equipment[5];
    public Skill[] Skills { get; private set; } = new Skill[5];
    public CombatantStat[] Stats { get; private set; } = new CombatantStat[9];

    public bool IsJoined { get; private set; }
    public CombatPosition Pos { get; private set; }

    public bool IsCharacterEquipped => Character != null;
    public bool IsEquipmentEquippedAt(int index) => Equipments[index] != null;
    public bool IsSkillEquippedAt(int index) => Skills[index] != null;

    public UnityAction<Combatant> onValueChanged;

    public Combatant()
    {
        for (int i = 0; i < Stats.Length; i++)
            Stats[i] = new CombatantStat();
    }

    public void JoinCombat(CombatPosition pos)
    {
        IsJoined = true;
        Pos = pos;
        Character.IsCombating = true;

        onValueChanged?.Invoke(this);
    }

    public void UnjoinCombat()
    {
        IsJoined = false;
        Pos = CombatPosition.Invalid;
        Character.IsCombating = false;

        ResetSkillBuffs();

        // NOTE :
        // ResetSkillBuffs 에서도 호출된다.
        onValueChanged?.Invoke(this);
    }

    public void EquipCharacter(Character character)
    {
        Character = character;
        for (int i = 0; i < Stats.Length; i++)
            Stats[i].characterStat = character.Stats[i];

        onValueChanged?.Invoke(this);
    }

    public Character ReleaseCharacter()
    {
        Character released = Character;
        Character = null;
        for (int i = 0; i < Stats.Length; i++)
            Stats[i].characterStat = released.Stats[i];

        onValueChanged?.Invoke(this);

        return released;
    }

    public void EquipEquipment(int index, Equipment equipment)
    {
        Equipments[index] = equipment;

        EquipmentBuff[] buffs = equipment.Buffs;
        for (int i = 0; i < buffs.Length; i++)
            Stats[(int)buffs[i].Info.Type].equipmentBuffs.Add(buffs[i]);

        List<RuneBuff> enchantedBuffs = equipment.EnchantedBuffs;
        for (int i = 0; i < enchantedBuffs.Count; i++)
            Stats[(int)enchantedBuffs[i].Type].equipmentEnchantedBuffs.Add(enchantedBuffs[i]);

        onValueChanged?.Invoke(this);
    }

    public Equipment ReleaseEquipment(int index)
    {
        Equipment released = Equipments[index];
        Equipments[index] = null;

        EquipmentBuff[] buffs = released.Buffs;
        for (int i = 0; i < buffs.Length; i++)
            Stats[(int)buffs[i].Info.Type].equipmentBuffs.Remove(buffs[i]);

        List<RuneBuff> enchantedBuffs = released.EnchantedBuffs;
        for (int i = 0; i < enchantedBuffs.Count; i++)
            Stats[(int)enchantedBuffs[i].Type].equipmentEnchantedBuffs.Remove(enchantedBuffs[i]);

        onValueChanged?.Invoke(this);

        return released;
    }

    public void EquipSkill(int index, Skill skill)
    {
        Skills[index] = skill;
        onValueChanged?.Invoke(this);
    }

    public Skill ReleaseSkill(int index)
    {
        Skill released = Skills[index];
        Skills[index] = null;

        onValueChanged?.Invoke(this);

        return released;
    }

    public void ResetSkillBuffs()
    {
        for (int i = 0; i < Stats.Length; i++)
            Stats[i].ResetSkillBuffs();

        onValueChanged?.Invoke(this);
    }
}
