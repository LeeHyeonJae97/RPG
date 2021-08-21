using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestTableSO _table;
    [SerializeField] private UserDataSO _userData;

    [SerializeField] private QuestEventChannelSO _questEventchannel;
    [SerializeField] private UIEventChannelSO _uiEventChannel;

    private void Start()
    {
        _questEventchannel.perform += Perform;
    }

    private void OnDestroy()
    {
        _questEventchannel.perform -= Perform;
    }

    public void OpenDailyQuestInfoUI()
    {
        _uiEventChannel.openDailyQuestInfoUI?.Invoke(_table.DailyQuests, ReceiveReward);
    }

    public void OpenRepeatableQuestInfoUI()
    {
        _uiEventChannel.openRepeatableQuestInfoUI?.Invoke(_table.RepeatableQuests, ReceiveReward);
    }

    public void Close()
    {
        _uiEventChannel.closeQuestInfoUI?.Invoke();
    }

    public void Perform(string id)
    {
        // NOTE :
        // Quest와 Index를 모두 반환하는 확장메소드 작성

        Quest quest = _table.DailyQuests.Find((quest) => quest.Id.Equals(id));
        if (quest != null)
        {
            quest.Perform();
            _uiEventChannel.updateDailyQuestSlot?.Invoke(_table.DailyQuests.IndexOf(quest as DailyQuest), quest);
        }

        quest = _table.RepeatableQuests.Find((quest) => quest.Id.Equals(id));
        if (quest != null)
        {
            quest.Perform();
            _uiEventChannel.updateRepeatableQuestSlot?.Invoke(_table.RepeatableQuests.IndexOf(quest as RepeatableQuest), quest);
        }
    }

    public void ReceiveReward(Quest quest)
    {
        Debug.Log(quest.Title);

        quest.ReceiveReward();
        _userData.Moneys[(int)quest.RewardType].amount += quest.Reward;
    }
}
