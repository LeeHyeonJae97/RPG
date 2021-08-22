using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "ScriptableObject/Monster")]
public class MonsterSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Preview { get; private set; }
    [field: SerializeField] public MonsterStat[] Stats { get; private set; }
    [field: SerializeField] public Skill[] Skills { get; private set; }
    [field: SerializeField] public Extension.RangeInt OutputExpRange { get; private set; }
    [field: SerializeField] public MoneyType OutputMoneyType { get; private set; }
    [field: SerializeField] public Extension.RangeInt OutputMoneyRange { get; private set; }
}
