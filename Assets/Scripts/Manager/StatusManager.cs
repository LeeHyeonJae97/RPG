using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public string Nickname { get; set; }

    [SerializeField] private UserDataSO _userData;

    [SerializeField] private UIEventChannelSO _uiEventChannel;
    [SerializeField] private StringEventChannelSO _alertModalEventChannel;

    private void Start()
    {
        _uiEventChannel.initStatusBarUI?.Invoke(_userData);
    }

    public void SaveNickname()
    {
        _userData.Nickname = Nickname;
        _uiEventChannel.updateNicknameUI?.Invoke(Nickname);
    }

    public bool Affordable(MoneyType type, int cost)
    {
        if (_userData.Moneys[(int)type].amount > cost)
        {
            return true;
        }
        else
        {
            _alertModalEventChannel.onEventRaised?.Invoke($"{_userData.Moneys[(int)type].MoneyName}가 부족합니다.");
            return false;
        }
    }

    public void EarnMoney(MoneyType type, int amount)
    {
        _userData.Moneys[(int)type].amount += amount;
        _uiEventChannel.updateMoneyUI?.Invoke(type, _userData.Moneys[(int)type].amount);
    }

    public void LoseMoney(MoneyType type, int amount)
    {
        _userData.Moneys[(int)type].amount -= amount;
        _uiEventChannel.updateMoneyUI?.Invoke(type, _userData.Moneys[(int)type].amount);
    }
}
