using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public delegate List<ILive> GetTarget(string tag, CombatTarget type, int count);

public class LiveCombatant : MonoBehaviour, ILive
{
    public float CurHp { get; private set; }
    public Combatant Combatant { get; private set; }

    public bool IsDead => !enabled;
    public bool IsEmpty => Combatant == null;

    private SpriteRenderer _sr;
    private GetTarget _getTarget;
    private UnityAction<Combatant> _die;

    private void Update()
    {
        for (int i = 0; i < Combatant.Skills.Length; i++)
        {
            if (Combatant.IsSkillEquippedAt(i))
            {
                Skill skill = Combatant.Skills[i];
                bool usable = skill.CalculateCooldown();
                //if (usable) skill.Use(Info.StatDic.Cast<Dictionary<string, Stat>>() as Dictionary<string, Stat>, _getTarget);
            }
        }
    }

    // 새롭게 스테이지 출전하는 경우 호출
    public void Init(Combatant info, GetTarget getTarget, UnityAction<Combatant> die)
    {
        Combatant = info;
        CurHp = Combatant.Stats[(int)StatType.Hp].Value;

        if (_sr == null) _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.sprite = Combatant.Character.Preview;

        _getTarget = getTarget;
        _die = die;

        enabled = true;
        gameObject.SetActive(true);
    }

    // 스테이지 클리어 또는 스테이지 재시작시 호출
    public void Init()
    {        
        Combatant.ResetSkillBuffs();
        CurHp = Combatant.Stats[(int)StatType.Hp].Value;

        enabled = true;
        gameObject.SetActive(true);
    }

    // 전투에서 나간 경우 호출
    public void Reset()
    {
        Combatant.ResetSkillBuffs();
        enabled = false;
        gameObject.SetActive(false);
    }

    public void Damaged(float damage)
    {
        CurHp -= damage;
        if (CurHp <= 0)
            Die();
    }

    public void Healed(float amount)
    {
        CurHp += amount;
        CurHp = Mathf.Min(CurHp, Combatant.Stats[(int)StatType.Hp].Value);
    }

    public void Buffed(StatType type, float value)
    {
        Combatant.Stats[(int)type].skillBuffs.Add(value);
    }

    public void Die()
    {
        Debug.Log("Die Combatant");
        enabled = false;
        _die?.Invoke(Combatant);
    }
}
