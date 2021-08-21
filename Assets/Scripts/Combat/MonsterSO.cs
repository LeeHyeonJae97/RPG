using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "ScriptableObject/Monster")]
public class MonsterSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Preview { get; private set; }
    [SerializeField] private MonsterStat[] _stats;
    public Dictionary<string, MonsterStat> StatDic { get; private set; } = new Dictionary<string, MonsterStat>();
    [field: SerializeField] public SkillSO[] Skills { get; private set; }
    [field: SerializeField] public Extension.RangeInt OutputExpRange { get; private set; }
    [field: SerializeField] public MoneyType OutputMoneyType { get; private set; }
    [field: SerializeField] public Extension.RangeInt OutputMoneyRange { get; private set; }

    public void Init()
    {
        StatDic = new Dictionary<string, MonsterStat>();
        for (int i = 0; i < _stats.Length; i++)
            StatDic.Add(_stats[i].StatName, _stats[i]);
    }
}
