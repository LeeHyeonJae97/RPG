using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Skill : IEquippable, IUpgradable, IDisasemblable
{
    public string Id { get; private set; }
    public int Level { get; private set; }
    [field: SerializeField] public SkillSO Info { get; private set; }
    public Damage Damage { get; private set; }
    public Damage Heal { get; private set; }
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

    private float _curCooldown;

    public string Description
    {
        get
        {
            string str = new string(Info.BuffsDescriptionFormat.ToCharArray());
            if (Info.HasDamage) str.Replace("{Damage}", Damage.CoefValue.ToString());
            if (Info.HasHeal) str.Replace("{Heal}", Heal.CoefValue.ToString());
            if (Info.HasBuffs)
            {
                for (int i = 0; i < Buffs.Length; i++)
                    str.Replace("{Buff}", Buffs[i].CoefValue.ToString());
            }

            return str;
        }
    }

    public string DescriptionWithNextLevel
    {
        get
        {
            string str = new string(Info.BuffsDescriptionFormat.ToCharArray());
            if (Info.HasDamage) str.Replace("{Damage}", $"{Damage.CoefValue} {Info.Damage.GetValue(Level + 1)}");
            if (Info.HasHeal) str.Replace("{Heal}", $"{Heal.CoefValue} {Info.Heal.GetValue(Level + 1)}");
            if (Info.HasBuffs)
            {
                for (int i = 0; i < Buffs.Length; i++)
                    str.Replace("{Value}", $"{Buffs[i].CoefValue} {Info.Buffs[i].GetValue(Level + 1)}");
            }

            return str;
        }
    }

    public UnityAction<Skill> onValueChanged;
    public UnityAction onRemoved;

    public Skill(SkillSO info)
    {
        Id = "";
        Level = 1;
        Info = info;
        if (Info.HasDamage) Damage = new Damage(info.Damage, Level);
        if (Info.HasHeal) Heal = new Damage(info.Heal, Level);
        if (Info.HasBuffs)
        {
            Buffs = new SkillBuff[info.Buffs.Length];
            for (int i = 0; i < Buffs.Length; i++)
                Buffs[i] = new SkillBuff(info.Buffs[i], Level);
        }
    }

    public Skill(string id, int level, SkillSO info)
    {
        Id = id;
        Level = level;
        Info = info;
        if (Info.HasDamage) Damage = new Damage(info.Damage, level);
        if (Info.HasHeal) Heal = new Damage(info.Heal, level);
        if (Info.HasBuffs)
        {
            Buffs = new SkillBuff[info.Buffs.Length];
            for (int i = 0; i < Buffs.Length; i++)
                Buffs[i] = new SkillBuff(info.Buffs[i], level);
        }
    }

    public void Init()
    {
        if (Info.HasDamage) Damage = new Damage(Info.Damage, 1);
        if (Info.HasHeal) Heal = new Damage(Info.Heal, 1);
        if (Info.HasBuffs)
        {
            Buffs = new SkillBuff[Info.Buffs.Length];
            for (int i = 0; i < Buffs.Length; i++)
                Buffs[i] = new SkillBuff(Info.Buffs[i], 1);
        }
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
            Buffs[i].Upgrade(Level);

        return true;
    }

    public int Disassembled()
    {
        return Info.DisassembleOutputAmountRange.Random();
    }

    public bool CalculateCooldown()
    {
        bool usable = _curCooldown <= 0;

        if (!usable)
            _curCooldown -= Time.deltaTime;
        return usable;
    }

    public void Use(GetStatValue getStatValue, GetTarget getTarget)
    {
        List<Live> targets = getTarget(Info.TargetTag, Info.TargetType, Info.TargetCount);
        if (targets == null || targets.Count == 0) return;

        _curCooldown = Info.Cooldown;

        if (Info.Type.HasFlag(SkillType.Damage))
        {
            float value = getStatValue(Damage.Info.CoefStatType) * Damage.CoefValue;
            for (int i = 0; i < targets.Count; i++)
                targets[i].Damaged(value);
        }
        if (Info.Type.HasFlag(SkillType.Heal))
        {
            float value = getStatValue(Heal.Info.CoefStatType) * Heal.CoefValue;
            for (int i = 0; i < targets.Count; i++)
                targets[i].Healed(value);
        }
        if (Info.Type.HasFlag(SkillType.Buff))
        {
            for (int i = 0; i < Buffs.Length; i++)
            {
                float value = getStatValue(Buffs[i].Info.CoefStatType) * Buffs[i].CoefValue;
                for (int j = 0; j < targets.Count; j++)
                    targets[j].Buffed(Buffs[i].Info.TargetStatType, value);
            }
        }
    }

    public void Reset()
    {
        _curCooldown = Info.Cooldown;
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
