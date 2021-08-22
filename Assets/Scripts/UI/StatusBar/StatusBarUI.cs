using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private TextMeshProUGUI _stageText;
    [SerializeField] private Image[] _moneyIcons;
    [SerializeField] private TextMeshProUGUI[] _moneyTexts;

    [SerializeField] private UIEventChannelSO _channel;

    private void Start()
    {
        _channel.initStatusBarUI += Init;
        _channel.updateNicknameUI += UpdateNicknameUI;
        _channel.updateStageUI += UpdateStageUI;
        _channel.updateMoneyUI += UpdateMoneyUI;
    }

    private void OnDestroy()
    {
        _channel.initStatusBarUI -= Init;
        _channel.updateNicknameUI -= UpdateNicknameUI;
        _channel.updateStageUI -= UpdateStageUI;
        _channel.updateMoneyUI -= UpdateMoneyUI;
    }

    public void Init(UserDataSO userData)
    {
        _nicknameText.text = userData.Nickname;
        _stageText.text = $"STAGE {userData.stage}";
        for (int i = 0; i < userData.Moneys.Length; i++)
        {
            Debug.Log(userData.Moneys[i].amount);

            _moneyIcons[i].sprite = userData.Moneys[i].Icon;
            _moneyTexts[i].text = userData.Moneys[i].amount.ToString();
        }
    }

    public void UpdateNicknameUI(string nickname)
    {
        _nicknameText.text = nickname;
    }

    public void UpdateStageUI(string name)
    {
        _stageText.text = name;
    }

    public void UpdateMoneyUI(MoneyType type, int amount)
    {
        _moneyTexts[(int)type].text = amount.ToString();
    }
}
