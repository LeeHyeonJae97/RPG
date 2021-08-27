using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public CharacterStat[] Stats;

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

    public UnityAction<Character> onValueChanged;    

    // NOTE :
    // 수정 필요
    public Character(string characterName, Sprite preview, CharacterStat[] stats)
    {
        Id = "";
        Name = characterName;
        Level = 1;
        Preview = preview;
        StatPoint = 99;

        Stats = stats;

        for (int i = 0; i < CombatPositionIndices.Length; i++)
            CombatPositionIndices[i] = -1;
    }

    public void Equipped(int presetIndex, int combatPositionIndex)
    {
        CombatPositionIndices[presetIndex] = combatPositionIndex;
        onValueChanged?.Invoke(this);
    }

    public void Released(int presetIndex)
    {
        CombatPositionIndices[presetIndex] = -1;
        onValueChanged?.Invoke(this);
    }

    public void InvestStatPoint(StatType type)
    {
        if (StatPoint > 0)
        {
            StatPoint--;
            Stats[(int)type].Invested();
            onValueChanged?.Invoke(this);
        }
    }

    public void Equipped(int presetIndex, int combatPositionIndex, int slotIndex) { }
}
