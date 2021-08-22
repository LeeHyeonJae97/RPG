using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroducedCharacterInfoUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;

    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI[] _statTexts;

    private void Start()
    {
        _channel.openIntroducedCharacterInfoUI += Open;
        _channel.closeIntroducedCharacterInfoUI += Close;
        _channel.updateIntroducedCharacterInfoUI += UpdateUI;
    }

    private void OnDestroy()
    {
        _channel.openIntroducedCharacterInfoUI -= Open;
        _channel.closeIntroducedCharacterInfoUI -= Close;
        _channel.updateIntroducedCharacterInfoUI -= UpdateUI;
    }

    public void Open(Sprite preview, CharacterStat[] stats)
    {
        UpdateUI(preview, stats);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void UpdateUI(Sprite preview, CharacterStat[] stats)
    {
        _previewImage.sprite = preview;

        if (_statTexts.Length != stats.Length)
            Debug.LogError("Error");

        for (int i = 0; i < _statTexts.Length; i++)
            _statTexts[i].text = $"{(StatType)i}    {stats[i].Value}";
    }
}
