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

    [SerializeField] private UserDataSO _userData;

    private void Start()
    {
        _userData.OnNicknameChanged += UpdateNicknameUI;
        _userData.OnStageValueChanged += UpdateStageUI;
        _userData.OnMoneyValueChanged += UpdateMoneyUI;
    }

    private void OnDestroy()
    {
        _userData.OnNicknameChanged -= UpdateNicknameUI;
        _userData.OnStageValueChanged -= UpdateStageUI;
        _userData.OnMoneyValueChanged -= UpdateMoneyUI;
    }

    private void UpdateNicknameUI(string nickname)
    {
        _nicknameText.text = nickname;
    }

    private void UpdateStageUI(string stageName)
    {
        _stageText.text = stageName;
    }

    private void UpdateMoneyUI(MoneyType type, int amount)
    {
        _moneyTexts[(int)type].text = amount.ToString();
    }
}
