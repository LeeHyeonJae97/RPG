using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public delegate List<ILive> GetTarget(string tag, CombatTarget type, int count);

public class LiveCombatant : MonoBehaviour, ILive
{
    public float CurHp { get; private set; }
    public Combatant Info { get; private set; }

    public bool IsDead => !enabled;
    public bool IsEmpty => Info == null;

    private SpriteRenderer _sr;
    private GetTarget _getTarget;
    private UnityAction<Combatant> _die;

    private void Update()
    {
        for (int i = 0; i < Info.Skills.Length; i++)
        {
            if (Info.IsSkillEquippedAt(i))
            {
                Skill skill = Info.Skills[i];
                bool usable = skill.CalculateCooldown();
                if (usable) skill.Use(Info.StatDic.Cast<Dictionary<string, Stat>>() as Dictionary<string, Stat>, _getTarget);
            }
        }
    }

    // 새롭게 스테이지 출전하는 경우 호출
    public void Init(Combatant info, GetTarget getTarget, UnityAction<Combatant> die)
    {
        Info = info;
        CurHp = Info.StatDic[Variables.StatNames[0]].Value;

        if (_sr == null)
            _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.sprite = Info.Character.Preview;

        _getTarget = getTarget;
        _die = die;

        gameObject.SetActive(true);
    }

    // 스테이지 클리어 또는 스테이지 재시작시 호출
    public void Init()
    {
        Info.ResetSkillBuffs();
        CurHp = Info.StatDic[Variables.StatNames[0]].Value;
    }

    // 전투에서 나간 경우 호출
    public void Reset()
    {
        Info = null;
        _sr.sprite = null;

        _getTarget = null;

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
        if (CurHp > Info.StatDic[Variables.STAT_MAXHP].Value)
            CurHp = Info.StatDic[Variables.STAT_MAXHP].Value;
    }

    public void Buffed(string statName, float value)
    {
        Info.StatDic[statName].skillBuffs.Add(value);
    }

    public void Die()
    {
        Debug.Log("Die Combatant");
        enabled = false;
        _die?.Invoke(Info);
    }
}
