using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SkillInfoUI : BaseInfoUI<Skill>
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _starText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private GameObject _equipButton;
    [SerializeField] private GameObject _releaseButton;
    [SerializeField] private Button _disassembleButton;
    [SerializeField] private Button _sellButton;

    private UnityAction _updateUI;

    private void Start()
    {
        _channel.openSkillInfoUI += Open;
        _channel.closeSkillInfoUI += Close;
    }

    private void OnDestroy()
    {
        _channel.openSkillInfoUI -= Open;
        _channel.closeSkillInfoUI -= Close;
    }

    public override void Open(Skill skill)
    {
        base.Open(skill);

        _updateUI = () => UpdateUI(skill);
        _channel.updateSkillInfoUI += _updateUI;
    }

    public override void Close()
    {
        base.Close();
        _channel.updateSkillInfoUI -= _updateUI;
    }

    public override void UpdateUI(Skill skill)
    {
        _previewImage.sprite = skill.Info.Preview;
        _starText.text = skill.Info.Star.ToString();
        _nameText.text = $"+{skill.Level} {skill.Info.Name}";
        _descriptionText.text = skill.Description;

        _equipButton.SetActive(!skill.IsEquipped);
        _releaseButton.SetActive(skill.IsEquipped);

        _disassembleButton.interactable = !skill.IsEquipped;
        _sellButton.interactable = !skill.IsEquipped;
    }
}
