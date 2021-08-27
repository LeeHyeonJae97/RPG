using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum SkillType { None = 0, Damage = 1, Heal = 2, Buff = 4 }

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/Item/Skill")]
public class SkillSO : ItemSO
{
    [field: Multiline] [field: SerializeField] public string BuffsDescriptionFormat { get; private set; }
    [field: EnumFlags] [field: SerializeField] public SkillType Type { get; private set; }
    [field: SerializeField] public string TargetTag { get; private set; }
    [field: SerializeField] public CombatTarget TargetType { get; private set; }
    [field: SerializeField] public int TargetCount { get; private set; }
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: ShowIf("HasDamage")] [field: SerializeField] public DamageInfo Damage { get; private set; }
    [field: ShowIf("HasHeal")] [field: SerializeField] public DamageInfo Heal { get; private set; }
    [field: ShowIf("HasBuffs")] [field: SerializeField] public SkillBuffInfo[] Buffs { get; private set; }

    public bool HasDamage => Type.HasFlag(SkillType.Damage);
    public bool HasHeal => Type.HasFlag(SkillType.Heal);
    public bool HasBuffs => Type.HasFlag(SkillType.Buff);
}
