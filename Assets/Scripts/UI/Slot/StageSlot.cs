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

    public void Init(StageSO info, UnityAction selectStage)
    {
        _nameText.text = info.Name;
        gameObject.SetActive(true);

        _slotButton.onClick.RemoveAllListeners();
        _slotButton.onClick.AddListener(selectStage);
    }
}
