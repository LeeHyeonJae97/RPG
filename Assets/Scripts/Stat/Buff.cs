using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Extension;

public enum CalculationType { Flat = 0, Percent = 1 }

[System.Serializable]
public class Buff
{
    [field: SerializeField] public string StatName { get; protected set; }
}

[System.Serializable]
public class EquipmentBuff : Buff
{
    public CalculationType Type { get; private set; }
    public float Value { get; private set; }

    public EquipmentBuff(EquipmentBuffInfo info, int level)
    {
        StatName = info.StatName;
        Type = info.Type;
        Value = float.Parse(new DataTable().Compute(string.Format(info.Formula, level), null).ToString());
    }

    public void Upgrade(string formula, int level)
    {
        Value = float.Parse(new DataTable().Compute(string.Format(formula, level), null).ToString());
    }
}

[System.Serializable]
public class EquipmentBuffInfo : Buff
{
    [field: SerializeField] public CalculationType Type { get; private set; }
    [field: SerializeField] public string Formula { get; private set; }

    public float GetValue(int level)
    {
        return float.Parse(new DataTable().Compute(string.Format(Formula, level), null).ToString());
    }
}

[System.Serializable]
public class SkillBuff : Buff
{
    public string CoefStatName { get; private set; }
    public float CoefValue { get; private set; }

    public SkillBuff(SkillBuffInfo info, int level)
    {
        StatName = info.StatName;
        CoefStatName = info.CoefStatName;
        CoefValue = float.Parse(new DataTable().Compute(string.Format(info.Formula, level), null).ToString());
    }

    public void Upgrade(string formula, int level)
    {
        CoefValue = float.Parse(new DataTable().Compute(string.Format(formula, level), null).ToString());
    }
}

[System.Serializable]
public class SkillBuffInfo : Buff
{
    [field: SerializeField] public string CoefStatName { get; private set; }
    [field: SerializeField] public string Formula { get; private set; }

    public float GetValue(int level)
    {
        return float.Parse(new DataTable().Compute(string.Format(Formula, level), null).ToString());
    }
}

// NOTE :
// 룬 버프 값을 랜덤하게 할지?
// 랜덤하게 하지 않는다면 Rune과 RuneSO를 따로 만들 필요가 없다.
// 인벤토리에도 Stackable하게 저장

[System.Serializable]
public class RuneBuff : Buff
{
    [field: SerializeField] public CalculationType Type { get; private set; }
    [field: SerializeField] public float Value { get; private set; }

    public RuneBuff(RuneBuff buff)
    {
        StatName = buff.StatName;
        Type = buff.Type;
        Value = buff.Value;
    }
}

[System.Serializable]
public class RelicBuff : Buff
{
    [field: SerializeField] public string Formula { get; private set; }
    public float Value { get; private set; }

    public void SetValue(int amount)
    {
        Value = float.Parse(new DataTable().Compute(string.Format(Formula, amount), null).ToString());
    }
}