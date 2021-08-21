using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LiveMonster : MonoBehaviour, ILive
{
    public float CurHp { get; private set; }
    public MonsterSO Info { get; private set; }
    private Skill[] _skills = new Skill[5];

    public bool IsDead => !enabled;
    public bool IsEmpty => Info == null;

    private SpriteRenderer _sr;
    private GetTarget _getTarget;
    private UnityAction<MonsterSO> _die;

    private void Update()
    {
        for (int i = 0; i < _skills.Length; i++)
        {
            if (_skills[i] == null) continue;

            bool usable = _skills[i].CalculateCooldown();
            if (usable)
            {
                Skill skill = _skills[i];
                SkillSO info = _skills[i].Info;

                // 쿨타임 초기화
                skill.curCooldown = info.Cooldown;

                // 타겟 선정
                List<ILive> targets = _getTarget?.Invoke(info.TargetTag, info.TargetType, info.TargetCount);

                switch (info.Type)
                {
                    case SkillType.Damage:
                        for (int j = 0; j < skill.Buffs.Length; j++)
                        {
                            float value = Info.StatDic[skill.Buffs[j].CoefStatName].Value * skill.Buffs[j].CoefValue;
                            for (int k = 0; k < targets.Count; k++)
                                targets[k].Damaged(value);
                        }
                        break;

                    case SkillType.Heal:
                        for (int j = 0; j < skill.Buffs.Length; j++)
                        {
                            float value = Info.StatDic[skill.Buffs[j].CoefStatName].Value * skill.Buffs[j].CoefValue;
                            for (int k = 0; k < targets.Count; k++)
                                targets[k].Healed(value);
                        }
                        break;

                    case SkillType.Buff:
                        for (int j = 0; j < skill.Buffs.Length; j++)
                        {
                            float value = Info.StatDic[skill.Buffs[j].CoefStatName].Value * skill.Buffs[j].CoefValue;
                            for (int k = 0; k < targets.Count; k++)
                                targets[k].Buffed(skill.Buffs[k].StatName, value);
                        }
                        break;
                }
            }
        }
    }

    // 새롭게 스테이지 출전하는 경우 호출
    public void Init(MonsterSO info, GetTarget getTarget, UnityAction<MonsterSO> die)
    {
        // NOTE :
        // 전부 초기화

        Info = info;

        info.Init();
        CurHp = Info.StatDic[Variables.StatNames[0]].Value;

        for (int i = 0; i < Info.Skills.Length; i++)
            _skills[i] = new Skill(Info.Skills[i]);

        if (_sr == null)
            _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.sprite = Info.Preview;

        this._getTarget = getTarget;
        _die = die;

        enabled = true;
        gameObject.SetActive(true);
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
        Debug.Log("Die Monster");
        enabled = false;
        _die?.Invoke(Info);
    }
}
