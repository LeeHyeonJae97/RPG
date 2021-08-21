using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Combatant
{
    public Character Character { get; private set; }
    public Equipment[] Equipments { get; private set; } = new Equipment[5];
    public Skill[] Skills { get; private set; } = new Skill[5];
    public Dictionary<string, CombatantStat> StatDic { get; private set; } = new Dictionary<string, CombatantStat>();

    public bool IsJoined { get; private set; }
    public CombatPosition Pos { get; private set; }

    public bool IsCharacterEquipped => Character != null;
    public bool IsEquipmentEquippedAt(int index) => Equipments[index] != null;
    public bool IsSkillEquippedAt(int index) => Skills[index] != null;

    public Combatant()
    {
        string[] statNames = Variables.StatNames;
        for (int i = 0; i < statNames.Length; i++)
            StatDic.Add(statNames[i], new CombatantStat());
    }

    public void JoinCombat(CombatPosition pos)
    {
        IsJoined = true;
        Pos = pos;
        Character.IsCombating = true;
    }

    public void UnjoinCombat()
    {
        IsJoined = false;
        Pos = CombatPosition.Invalid;
        Character.IsCombating = false;

        ResetSkillBuffs();
    }

    public void EquipCharacter(Character character)
    {
        Character = character;
        string[] statNames = Variables.StatNames;
        for (int i = 0; i < statNames.Length; i++)
            StatDic[statNames[i]].characterStat = character.StatDic[statNames[i]];
    }

    public Character ReleaseCharacter()
    {
        Character released = Character;
        Character = null;
        string[] statNames = Variables.StatNames;
        for (int i = 0; i < statNames.Length; i++)
            StatDic[statNames[i]].characterStat = null;
        return released;
    }

    public void EquipEquipment(int index, Equipment equipment)
    {
        Equipments[index] = equipment;

        EquipmentBuff[] buffs = equipment.Buffs;
        for (int i = 0; i < buffs.Length; i++)
            StatDic[buffs[i].StatName].equipmentBuffs.Add(buffs[i]);

        List<RuneBuff> enchantedBuffs = equipment.EnchantedBuffs;
        for (int i = 0; i < enchantedBuffs.Count; i++)
            StatDic[enchantedBuffs[i].StatName].equipmentEnchantedBuffs.Add(enchantedBuffs[i]);
    }

    public Equipment ReleaseEquipment(int index)
    {
        Equipment released = Equipments[index];
        Equipments[index] = null;

        EquipmentBuff[] buffs = released.Buffs;
        for (int i = 0; i < buffs.Length; i++)
            StatDic[buffs[i].StatName].equipmentBuffs.Remove(buffs[i]);

        List<RuneBuff> enchantedBuffs = released.EnchantedBuffs;
        for (int i = 0; i < enchantedBuffs.Count; i++)
            StatDic[enchantedBuffs[i].StatName].equipmentEnchantedBuffs.Remove(enchantedBuffs[i]);

        return released;
    }

    public void EquipSkill(int index, Skill skill)
    {
        Skills[index] = skill;
    }

    public Skill ReleaseSkill(int index)
    {
        Skill released = Skills[index];
        Skills[index] = null;
        return released;
    }

    public void ResetSkillBuffs()
    {
        CombatantStat[] stats = StatDic.Values.ToArray();
        for (int i = 0; i < stats.Length; i++)
            stats[i].ResetSkillBuffs();
    }
}