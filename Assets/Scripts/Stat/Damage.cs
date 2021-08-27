using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[System.Serializable]
public class Damage
{
    public DamageInfo Info { get; private set; }
    public float CoefValue { get; private set; }

    public Damage(DamageInfo info, int level)
    {
        Info = info;
        CoefValue = float.Parse(new DataTable().Compute(string.Format(info.Formula, level), null).ToString());
    }

    public void Upgrade(int level)
    {
        CoefValue = float.Parse(new DataTable().Compute(string.Format(Info.Formula, level), null).ToString());
    }
}

[System.Serializable]
public class DamageInfo
{
    [field: SerializeField] public StatType CoefStatType { get; protected set; }
    [field: SerializeField] public string Formula { get; private set; }

    public float GetValue(int level)
    {
        return float.Parse(new DataTable().Compute(string.Format(Formula, level), null).ToString());
    }
}
