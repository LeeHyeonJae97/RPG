using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : IEquippable
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }
    public Sprite Preview { get; private set; }
    private int _exp;
    public int Exp
    {
        get { return _exp; }

        set
        {
            _exp = value;
            if (_exp >= Variables.MaxExps[Level])
            {
                Level++;
                StatPoint++;
                Exp = 0;
            }
        }
    }
    public int StatPoint { get; private set; }
    public Dictionary<string, CharacterStat> StatDic { get; private set; } = new Dictionary<string, CharacterStat>();

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
    public bool IsCombating { get; set; } = false;
    public int[] CombatPositionIndices { get; private set; } = new int[5];

    // NOTE :
    // 수정 필요
    public Character(string characterName, int level)
    {
        Name = characterName;
        Level = level;

        for (int i = 0; i < CombatPositionIndices.Length; i++)
            CombatPositionIndices[i] = -1;
    }

    // NOTE :
    // 수정 필요
    public Character(string characterName, CharacterStat[] stats)
    {
        Name = characterName;
        Level = 1;
        StatPoint = 99;

        for (int i = 0; i < stats.Length; i++)
            StatDic.Add(stats[i].StatName, stats[i]);

        for (int i = 0; i < CombatPositionIndices.Length; i++)
            CombatPositionIndices[i] = -1;
    }

    public void Equipped(int presetIndex, int combatPositionIndex)
    {
        CombatPositionIndices[presetIndex] = combatPositionIndex;
    }

    public void Released(int presetIndex)
    {
        CombatPositionIndices[presetIndex] = -1;
    }

    public void InvestStatPoint(string statName)
    {
        StatPoint--;
        StatDic[statName].Invested();
    }

    public void Equipped(int presetIndex, int combatPositionIndex, int slotIndex) { }
}
