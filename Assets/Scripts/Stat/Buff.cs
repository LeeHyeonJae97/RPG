using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Extension;

public enum CalculationType { Flat = 0, Percent = 1 }

[System.Serializable]
public class EquipmentBuff
{
    public EquipmentBuffInfo Info { get; private set; }
    public float Value { get; private set; }

    public EquipmentBuff(EquipmentBuffInfo info, int level)
    {
        Info = info;
        Value = float.Parse(new DataTable().Compute(string.Format(info.Formula, level), null).ToString());
    }

    public void Upgrade(string formula, int level)
    {
        Value = float.Parse(new DataTable().Compute(string.Format(formula, level), null).ToString());
    }
}

[System.Serializable]
public class EquipmentBuffInfo
{
    [field: SerializeField] public StatType Type { get; private set; }
    [field: SerializeField] public CalculationType CalType { get; private set; }
    [field: SerializeField] public string Formula { get; private set; }

    public float GetValue(int level)
    {
        return float.Parse(new DataTable().Compute(string.Format(Formula, level), null).ToString());
    }
}

[System.Serializable]
public class SkillBuff
{
    public SkillBuffInfo Info { get; private set; }
    public float CoefValue { get; private set; }

    public SkillBuff(SkillBuffInfo info, int level)
    {
        Info = info;
        CoefValue = float.Parse(new DataTable().Compute(string.Format(info.Formula, level), null).ToString());
    }

    public void Upgrade(string formula, int level)
    {
        CoefValue = float.Parse(new DataTable().Compute(string.Format(formula, level), null).ToString());
    }
}

[System.Serializable]
public class SkillBuffInfo
{
    [field: SerializeField] public StatType Type { get; private set; }
    [field: SerializeField] public StatType CoefStatType { get; protected set; }
    [field: SerializeField] public string Formula { get; private set; }

    public float GetValue(int level)
    {
        return float.Parse(new DataTable().Compute(string.Format(Formula, level), null).ToString());
    }
}

// NOTE :
// �� ���� ���� �����ϰ� ����?
// �����ϰ� ���� �ʴ´ٸ� Rune�� RuneSO�� ���� ���� �ʿ䰡 ����.
// �κ��丮���� Stackable�ϰ� ����

[System.Serializable]
public class RuneBuff
{
    [field: SerializeField] public StatType Type { get; private set; }
    [field: SerializeField] public CalculationType CalType { get; private set; }
    [field: SerializeField] public float Value { get; private set; }

    public RuneBuff(RuneBuff buff)
    {
        Type = buff.Type;
        CalType = buff.CalType;
        Value = buff.Value;
    }
}

[System.Serializable]
public class RelicBuff
{
    [field: SerializeField] public StatType Type { get; private set; }
    [field: SerializeField] public string Formula { get; private set; }
    public float Value { get; private set; }

    public void SetValue(int amount)
    {
        Value = float.Parse(new DataTable().Compute(string.Format(Formula, amount), null).ToString());
    }
}