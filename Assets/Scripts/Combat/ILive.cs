using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILive
{
    bool IsDead { get; }
    bool IsEmpty { get; }
    void Damaged(float damage);
    void Healed(float amount);
    void Buffed(StatType type, float value);
    void Die();
}
