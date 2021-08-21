using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class WorldSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] protected Button _slotButton;

    public void Init(WorldSO info, UnityAction onClick)
    {
        _nameText.text = info.Name;

        _slotButton.onClick.AddListener(onClick);
    }
}
