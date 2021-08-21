using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : IEquippable, IUpgradable, IDisasemblable
{
    public string Id { get; private set; }
    public int Level { get; private set; }
    public SkillSO Info { get; private set; }
    public SkillBuff[] Buffs { get; private set; }

    public bool IsEquipped
    {
        get
        {
            for (int i = 0; i < CombatPositionIndices.Length; i++)
            {
                if (CombatPositionIndices[i] != -1)
                    return true;
            }

            return false;
        }
    }
    public int[] CombatPositionIndices { get; private set; } = new int[5];
    public int[] SlotIndices { get; private set; } = new int[5];

    public float curCooldown;

    public string Description
    {
        get
        {
            string str = new string(Info.BuffsDescriptionFormat.ToCharArray());
            for (int i = 0; i < Buffs.Length; i++)
                str.Replace("{Value}", Buffs[i].CoefValue.ToString());

            return str;
        }
    }

    public string DescriptionWithNextLevel
    {
        get
        {
            string str = new string(Info.BuffsDescriptionFormat.ToCharArray());
            for (int i = 0; i < Buffs.Length; i++)
                str.Replace("{Value}", $"{Buffs[i].CoefValue} {Info.Buffs[i].GetValue(Level + 1)}");

            return str;
        }
    }

    public Skill(SkillSO info)
    {
        Id = "";
        Level = 1;
        Info = info;
        Buffs = new SkillBuff[info.Buffs.Length];
        for (int i = 0; i < Buffs.Length; i++)
            Buffs[i] = new SkillBuff(info.Buffs[i], 1);
    }

    public Skill(string id, int level, SkillSO info)
    {
        Id = id;
        Level = level;
        Info = info;
        Buffs = new SkillBuff[info.Buffs.Length];
        for (int i = 0; i < Buffs.Length; i++)
            Buffs[i] = new SkillBuff(info.Buffs[i], level);
    }

    public void Equipped(int presetIndex, int combatPositionIndex, int slotIndex)
    {
        CombatPositionIndices[presetIndex] = combatPositionIndex;
        SlotIndices[presetIndex] = slotIndex;
    }

    public void Released(int presetIndex)
    {
        CombatPositionIndices[presetIndex] = -1;
        SlotIndices[presetIndex] = -1;
    }

    public bool Upgraded()
    {
        Level++;
        for (int i = 0; i < Buffs.Length; i++)
            Buffs[i].Upgrade(Info.Buffs[i].Formula, Level);

        return true;
    }

    public int Disassembled()
    {
        return Info.DisassembleOutputAmountRange.Random();
    }

    public bool CalculateCooldown()
    {
        bool usable = curCooldown <= 0;

        if (!usable)
            curCooldown -= Time.deltaTime;
        return usable;
    }

    public virtual void Use(Dictionary<string, Stat> statDic, GetTarget getTarget)
    {
    }

    public void Reset()
    {
        curCooldown = Info.Cooldown;
    }

    public void Equipped(int presetIndex, int combatPositionIndex) { }
}

public enum CrowdControlType { Stunned }

public struct CrowdControl
{
    public CrowdControlType Type { get; private set; }
    public float Duration { get; private set; }

    public CrowdControl(CrowdControlType type, float duration)
    {
        Type = type;
        Duration = duration;
    }
}
