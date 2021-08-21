using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Damage, Heal, Buff }

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/Item/Skill")]
public class SkillSO : ItemSO
{
    [field: Multiline] [field: SerializeField] public string BuffsDescriptionFormat { get; private set; }
    [field: SerializeField] public SkillType Type { get; private set; }
    [field: SerializeField] public string TargetTag { get; private set; }
    [field: SerializeField] public CombatTarget TargetType { get; private set; }
    [field: SerializeField] public int TargetCount { get; private set; }
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: SerializeField] public SkillBuffInfo[] Buffs { get; private set; }
}
