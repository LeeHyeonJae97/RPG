using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuneInfoUI : BaseInfoUI<Rune>
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _starText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _enchantSuccessPercentText;
    [SerializeField] private TextMeshProUGUI _buffsText;

    private void Start()
    {
        _channel.openRuneInfoUI += Open;
        _channel.closeRuneInfoUI += Close;
    }

    private void OnDestroy()
    {
        _channel.openRuneInfoUI -= Open;
        _channel.closeRuneInfoUI -= Close;
    }

    public override void UpdateUI(Rune rune)
    {
        _previewImage.sprite = rune.Info.Preview;
        _starText.text = rune.Info.Star.ToString();
        _nameText.text = rune.Info.Name;
        _enchantSuccessPercentText.text = $"연성 성공 확률 {rune.Info.EnchantSuccessPercent}";
        _buffsText.text = rune.BuffsDescription;
    }
}
