using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public delegate List<Live> GetTarget(string tag, CombatTarget type, int count);
public delegate float GetStatValue(StatType type);

public class LiveCombatant : Live
{
    public Combatant Combatant { get; private set; }
    public override bool IsEmpty => Combatant == null;

    private UnityAction<Combatant> _die;

    private void Update()
    {
        for (int i = 0; i < Combatant.Skills.Length; i++)
        {
            if (Combatant.IsSkillEquippedAt(i))
            {
                Skill skill = Combatant.Skills[i];
                bool usable = skill.CalculateCooldown();
                if (usable) skill.Use(GetStatValue, _getTarget);
            }
        }
    }

    // 새롭게 스테이지 출전하는 경우 호출
    public void Init(Combatant info, GetTarget getTarget, UnityAction<Combatant> die)
    {
        Combatant = info;
        CurHp = GetStatValue(StatType.Hp);

        if (_sr == null) _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.sprite = Combatant.Character.Preview;

        _getTarget = getTarget;
        _die = die;

        enabled = true;
        gameObject.SetActive(true);

        onCurHpRatioValueChanged?.Invoke(CurHp / GetStatValue(StatType.Hp));
    }

    // 스테이지 클리어 또는 스테이지 재시작시 호출
    public void Init()
    {
        Combatant.ResetSkillBuffs();
        CurHp = GetStatValue(StatType.Hp);

        enabled = true;
        gameObject.SetActive(true);

        onCurHpRatioValueChanged?.Invoke(CurHp / GetStatValue(StatType.Hp));
    }

    // 전투에서 나간 경우 호출
    public void Reset()
    {
        Combatant.ResetSkillBuffs();
        enabled = false;
        gameObject.SetActive(false);
    }

    public override void Buffed(StatType type, float value)
    {
        Combatant.Stats[(int)type].skillBuffs.Add(value);
        onCurHpRatioValueChanged?.Invoke(CurHp / GetStatValue(StatType.Hp));
    }

    public override void Die()
    {
        Debug.Log("Die Combatant");
        enabled = false;
        _die?.Invoke(Combatant);
    }

    public override float GetStatValue(StatType type)
    {
        return Combatant.Stats[(int)type].Value;
    }
}
