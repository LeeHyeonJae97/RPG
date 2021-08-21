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

    public void Init(Quest quest, UnityAction receiveReward)
    {
        UpdateUI(quest);
        gameObject.SetActive(true);

        _receiveRewardButton.onClick.RemoveAllListeners();
        _receiveRewardButton.onClick.AddListener(() => receiveReward?.Invoke());
        _receiveRewardButton.onClick.AddListener(() => UpdateUI(quest));
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
