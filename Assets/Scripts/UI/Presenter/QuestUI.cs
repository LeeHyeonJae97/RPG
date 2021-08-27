using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private QuestTableSO _table;
    [SerializeField] private UserDataSO _userData;

    [SerializeField] private Transform _slotHolder;
    [SerializeField] private QuestSlot _slotPrefab;

    public void OpenDailyQuest()
    {
        Init(_table.DailyQuests.Cast<Quest>().ToList(), ReceiveReward);
        gameObject.SetActive(true);
    }

    public void OpenRepeatableQuest()
    {
        Init(_table.RepeatableQuests.Cast<Quest>().ToList(), ReceiveReward);
        gameObject.SetActive(true);
    }

    private void Init(List<Quest> quests, UnityAction<Quest> receiveReward)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (i < _slotHolder.childCount)
            {
                int index = i;

                QuestSlot slot = _slotHolder.GetChild(index).GetComponent<QuestSlot>();
                slot.Init(() => receiveReward(quests[index]));
                slot.UpdateUI(quests[index]);

                quests[index].onValueChanged = slot.UpdateUI;
            }
            else
            {
                int index = i;

                QuestSlot slot = Instantiate(_slotPrefab, _slotHolder);
                slot.Init(() => receiveReward(quests[index]));
                slot.UpdateUI(quests[index]);

                quests[index].onValueChanged = slot.UpdateUI;
            }
        }
        for (int i = quests.Count; i < _slotHolder.childCount; i++)
        {
            _slotHolder.GetChild(i).gameObject.SetActive(false);
            quests[i].onValueChanged = null;
        }
    }

    public void Close()
    {
        List<DailyQuest> dailyQuests = _table.DailyQuests;
        for (int i = 0; i < dailyQuests.Count; i++)
            dailyQuests[i].onValueChanged = null;

        List<RepeatableQuest> repeatableQuests = _table.RepeatableQuests;
        for (int i = 0; i < repeatableQuests.Count; i++)
            repeatableQuests[i].onValueChanged = null;

        gameObject.SetActive(false);
    }

    public void ReceiveReward(Quest quest)
    {
        Debug.Log(quest.Title);

        quest.ReceiveReward();
        _userData.EarnMoney(quest.RewardType, quest.Reward);
    }

    private void UpdateSlotUI(int index, Quest quest)
    {
        _slotHolder.GetChild(index).GetComponent<QuestSlot>().UpdateUI(quest);
    }
}
