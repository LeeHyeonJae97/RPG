using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RuneType { Atk, Def, Etc }

[CreateAssetMenu(fileName = "Rune", menuName = "ScriptableObject/Item/Rune")]
public class RuneSO : ItemSO
{
    [field: SerializeField] public RuneType Type { get; private set; }
    [field: SerializeField] public RuneBuff[] Buffs { get; private set; }
    [field: SerializeField] public int EnchantSuccessPercent { get; private set; }
}
