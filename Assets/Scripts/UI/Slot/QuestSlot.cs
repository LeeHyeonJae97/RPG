using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private GameObject _unperformableCoverImage;
    [SerializeField] private Button _receiveRewardButton;

    private UnityAction _onClick;

    private void Start()
    {
        _receiveRewardButton.onClick.AddListener(_onClick);
    }

    public void Init(UnityAction receiveReward)
    {
        _onClick = receiveReward;
        gameObject.SetActive(true);
    }

    public void UpdateUI(Quest quest)
    {
        _nameText.text = quest.Title;
        _countText.text = $"{quest.Current}/{quest.Required}";
        _rewardText.text = quest.Reward.ToString();

        _unperformableCoverImage.SetActive(!quest.Performable);
        _receiveRewardButton.interactable = quest.Clearable;
    }
}
