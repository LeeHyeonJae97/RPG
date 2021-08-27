using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObject/World/Stage")]
public class StageSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int RecommendedCombatPower { get; private set; }
    [field: SerializeField] public Sprite Background { get; private set; }
    [field: SerializeField] public Wave[] Waves { get; private set; }
}

[System.Serializable]
public class Wave
{
    [field: SerializeField] public MonsterSO[] Monsters { get; private set; } = new MonsterSO[3];
}
