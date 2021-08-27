using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LiveMonster : Live
{
    public MonsterSO Monster { get; private set; }
    public override bool IsEmpty => Monster == null;

    private UnityAction<MonsterSO> _die;

    private void Update()
    {
        for (int i = 0; i < Monster.Skills.Length; i++)
        {
            if (Monster.Skills[i] != null)
            {
                Skill skill = Monster.Skills[i];
                bool usable = skill.CalculateCooldown();
                if (usable) skill.Use(GetStatValue, _getTarget);
            }
        }
    }

    // 새롭게 스테이지 출전하는 경우 호출
    public void Init(MonsterSO monster, GetTarget getTarget, UnityAction<MonsterSO> die)
    {
        Monster = monster;
        CurHp = GetStatValue(StatType.Hp);

        if (_sr == null) _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.sprite = Monster.Preview;

        _getTarget = getTarget;
        _die = die;

        enabled = true;
        gameObject.SetActive(true);

        onCurHpRatioValueChanged?.Invoke(CurHp / GetStatValue(StatType.Hp));
    }

    public void Init()
    {
        Monster.ResetSkillBuffs();
        CurHp = GetStatValue(StatType.Hp);

        enabled = true;
        gameObject.SetActive(true);

        onCurHpRatioValueChanged?.Invoke(CurHp / GetStatValue(StatType.Hp));
    }

    public override void Buffed(StatType type, float value)
    {
        Monster.Stats[(int)type].skillBuffs.Add(value);
        onCurHpRatioValueChanged?.Invoke(CurHp / GetStatValue(StatType.Hp));
    }

    public override void Die()
    {
        Debug.Log("Die Monster");
        enabled = false;
        _die?.Invoke(Monster);
    }

    public override float GetStatValue(StatType type)
    {
        return Monster.Stats[(int)type].Value;
    }
}
