using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat
{
    public float Value { get; set; }

    public CharacterStat(float value)
    {
        Value = value;
    }

    public void Invested()
    {
        Value += 1;
    }
}

[System.Serializable]
public class CombatantStat
{
    public CharacterStat characterStat;
    public List<EquipmentBuff> equipmentBuffs = new List<EquipmentBuff>();
    public List<RuneBuff> equipmentEnchantedBuffs = new List<RuneBuff>();
    public List<float> skillBuffs = new List<float>();

    public float Value { get { return BaseValue + EquipmentBuffValue + SkillBuffValue; } set { } }

    public float BaseValue { get { return characterStat == null ? 0 : characterStat.Value; } }

    public float EquipmentBuffValue
    {
        get
        {
            if (equipmentBuffs.Count == 0)
                return 0;

            float value = BaseValue;
            float finalValue = 0;
            for (int i = 0; i < equipmentBuffs.Count; i++)
            {
                switch (equipmentBuffs[i].Info.CalType)
                {
                    case CalculationType.Flat:
                        finalValue += equipmentBuffs[i].Value;
                        break;

                    case CalculationType.Percent:
                        finalValue += value * equipmentBuffs[i].Value;
                        break;
                }
            }
            for (int i = 0; i < equipmentEnchantedBuffs.Count; i++)
            {
                switch (equipmentEnchantedBuffs[i].CalType)
                {
                    case CalculationType.Flat:
                        finalValue += equipmentEnchantedBuffs[i].Value;
                        break;

                    case CalculationType.Percent:
                        finalValue += value * equipmentEnchantedBuffs[i].Value;
                        break;
                }
            }

            return finalValue;
        }
    }

    public float SkillBuffValue
    {
        get
        {
            float value = 0;
            for (int i = 0; i < skillBuffs.Count; i++)
                value += skillBuffs[i];

            return value;
        }
    }

    public void ResetSkillBuffs()
    {
        skillBuffs = new List<float>();
    }
}

[System.Serializable]
public class MonsterStat
{
    [field: SerializeField] public float BaseValue { get; private set; }
    [HideInInspector] public List<float> skillBuffs = new List<float>();

    public float Value
    {
        get
        {
            float value = BaseValue;
            for (int i = 0; i < skillBuffs.Count; i++)
                value += skillBuffs[i];

            return value;
        }
    }
}