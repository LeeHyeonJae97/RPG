using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObject/UserData")]
public class UserDataSO : ScriptableObject
{
    [SerializeField] private string _nickname;
    public string Nickname
    {
        get { return _nickname; }

        set
        {
            _nickname = value;
            OnNicknameChanged?.Invoke(value);
        }
    }

    [SerializeField] public int stageId;    

    [SerializeField] private string _stageName;
    public string StageName
    {
        get { return _stageName; }

        set
        {
            _stageName = value;
            OnStageValueChanged?.Invoke(value);
        }
    }

    [SerializeField] private MoneySO[] _moneys = new MoneySO[5];

    public UnityAction<string> OnNicknameChanged;
    public UnityAction<string> OnStageValueChanged;
    public UnityAction<MoneyType, int> OnMoneyValueChanged;

    public void Load()
    {
        // 세이브 파일 로드
        
        for (int i = 0; i < _moneys.Length; i++)       
            _moneys[i].amount = 9999;           
    }

    public bool Affordable(MoneyType type, int cost)
    {
        if (_moneys[(int)type].amount > cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EarnMoney(MoneyType type, int amount)
    {
        _moneys[(int)type].amount += amount;
        OnMoneyValueChanged(type, _moneys[(int)type].amount);
    }

    public void LoseMoney(MoneyType type, int amount)
    {
        _moneys[(int)type].amount -= amount;
        OnMoneyValueChanged(type, _moneys[(int)type].amount);
    }
}
