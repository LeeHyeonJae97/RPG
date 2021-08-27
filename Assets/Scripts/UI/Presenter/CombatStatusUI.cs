using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatStatusUI : MonoBehaviour
{
    [SerializeField] private Image _hpBarFillImage;
    [SerializeField] private Live _live;

    private void Start()
    {
        _live.onCurHpRatioValueChanged += UpdateHpBarUI;
    }

    private void OnDestroy()
    {
        _live.onCurHpRatioValueChanged -= UpdateHpBarUI;
    }

    public void UpdateHpBarUI(float ratio)
    {
        _hpBarFillImage.fillAmount = ratio;
    }
}
