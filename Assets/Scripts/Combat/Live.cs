using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Live : MonoBehaviour
{
    public float CurHp { get; protected set; }

    protected SpriteRenderer _sr;
    protected GetTarget _getTarget;
    public UnityAction<float> onCurHpRatioValueChanged;

    public bool IsDead => !enabled;

    public abstract bool IsEmpty { get; }

    public abstract void Buffed(StatType type, float value);
    public abstract void Die();
    public abstract float GetStatValue(StatType type);

    public void Damaged(float damage)
    {
        CurHp -= damage;
        if (CurHp <= 0) Die();

        onCurHpRatioValueChanged?.Invoke(CurHp / GetStatValue(StatType.Hp));
    }

    public void Healed(float amount)
    {
        CurHp += amount;
        CurHp = Mathf.Min(CurHp, GetStatValue(StatType.Hp));

        onCurHpRatioValueChanged?.Invoke(CurHp / GetStatValue(StatType.Hp));
    }
}
