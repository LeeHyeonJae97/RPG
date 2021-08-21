using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CharacterInfoUI : BaseInfoUI<Character>
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _statPointText;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private Image _expBarFillImage;
    [SerializeField] private TextMeshProUGUI[] _statTexts;
    [SerializeField] private Button[] _investButtons;
    [SerializeField] private GameObject _equipButton;
    [SerializeField] private GameObject _releaseButton;
    [SerializeField] private Button _fireButton;

    private UnityAction _updateCharacterInfoUI;
    private UnityAction _updateCombatingCharacterInfoUI;

    private void Start()
    {
        _channel.openCharacterInfoUI += Open;
        _channel.closeCharacterInfoUI += Close;
    }

    private void OnDestroy()
    {
        _channel.openCharacterInfoUI -= Open;
        _channel.closeCharacterInfoUI -= Close;
    }

    public override void Open(Character character)
    {
        base.Open(character);

        _updateCharacterInfoUI = () => UpdateUI(character);
        _channel.updateCharacterInfoUI += _updateCharacterInfoUI;
        if (character.IsCombating)
        {
            _updateCombatingCharacterInfoUI = () => UpdateUI(character);
            _channel.updateCombatingCharacterInfoUI += _updateCombatingCharacterInfoUI;
        }
    }

    public override void Close()
    {
        base.Close();
        _channel.updateCharacterInfoUI -= _updateCharacterInfoUI;
        _channel.updateCombatingCharacterInfoUI -= _updateCombatingCharacterInfoUI;
    }

    public override void UpdateUI(Character character)
    {
        _previewImage.sprite = character.Preview;
        _nameText.text = character.Name;
        _levelText.text = $"Lv.{character.Level}";
        _statPointText.text = $"SP {character.StatPoint}";
        int exp = character.Exp;
        int maxExp = Variables.MaxExps[character.Level];
        _expText.text = $"{exp}/{maxExp}";
        _expBarFillImage.fillAmount = exp / maxExp;

        for (int i = 0; i < _statTexts.Length; i++)
            _statTexts[i].text =character.StatDic[Variables.StatNames[i]].Value.ToString();

        for (int i = 0; i < _investButtons.Length; i++)
            _investButtons[i].interactable = character.StatPoint > 0;
    }
}
