using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private UIEventChannelSO _channel;
    [SerializeField] private Transform _slotHolder;
    [SerializeField] private QuestSlot _slotPrefab;

    private void Start()
    {
        _channel.openDailyQuestInfoUI += Open;
        _channel.openRepeatableQuestInfoUI += Open;
        _channel.closeQuestInfoUI += Close;
    }

    private void OnDestroy()
    {
        _channel.openDailyQuestInfoUI -= Open;
        _channel.openRepeatableQuestInfoUI -= Open;
        _channel.closeQuestInfoUI -= Close;
    }

    public void Open(List<DailyQuest> quests, UnityAction<Quest> receiveReward)
    {
        Init(quests.Cast<Quest>().ToList(), receiveReward);

        _channel.updateDailyQuestSlot += UpdateSlotUI;
        _channel.updateRepeatableQuestSlot -= UpdateSlotUI;

        gameObject.SetActive(true);
    }

    public void Open(List<RepeatableQuest> quests, UnityAction<Quest> receiveReward)
    {
        Init(quests.Cast<Quest>().ToList(), receiveReward);

        _channel.updateDailyQuestSlot -= UpdateSlotUI;
        _channel.updateRepeatableQuestSlot += UpdateSlotUI;

        gameObject.SetActive(true);
    }

    private void Init(List<Quest> quests, UnityAction<Quest> receiveReward)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (i < _slotHolder.childCount)
            {
                int index = i;
                _slotHolder.GetChild(index).GetComponent<QuestSlot>().Init(quests[index], () => receiveReward(quests[index]));
            }
            else
            {
                int index = i;
                Instantiate(_slotPrefab, _slotHolder).Init(quests[index], () => receiveReward(quests[index]));
            }
        }
        for (int i = quests.Count; i < _slotHolder.childCount; i++)
            _slotHolder.GetChild(i).gameObject.SetActive(false);
    }

    public void Close()
    {
        _channel.updateDailyQuestSlot -= UpdateSlotUI;
        _channel.updateRepeatableQuestSlot -= UpdateSlotUI;

        gameObject.SetActive(false);
    }

    public void UpdateSlotUI(int index, Quest quest)
    {
        _slotHolder.GetChild(index).GetComponent<QuestSlot>().UpdateUI(quest);
    }
}
