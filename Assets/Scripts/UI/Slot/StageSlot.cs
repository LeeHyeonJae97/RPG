using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Button _slotButton;

    private UnityAction _onClick;

    private void Start()
    {
        _slotButton.onClick.AddListener(_onClick);
    }

    public void Init(StageSO info, UnityAction onClick)
    {
        _nameText.text = info.Name;
        _onClick = onClick;
        gameObject.SetActive(true);
    }
}
