using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicSlot : MonoBehaviour
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _buffText;

    public void UpdateUI(RelicSO relic)
    {
        _previewImage.sprite = relic.Preview;
        _nameText.text = relic.Name;
        _amountText.text = $"{relic.Amount} / {relic.MaxAmount}";
        _buffText.text = $"{relic.Buff.Type} {relic.Buff.Value}% ¡ı∞°";
    }
}
