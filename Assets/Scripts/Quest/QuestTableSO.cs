using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestTable", menuName = "ScriptableObject/QuestTable")]
public class QuestTableSO : ScriptableObject
{
    [field: SerializeField] public List<DailyQuest> DailyQuests { get; private set; }
    [field: SerializeField] public List<RepeatableQuest> RepeatableQuests { get; private set; }

    public void Load()
    {
        // ���̺�� ���� �ε�

        // NOTE :
        // Daily�� cleared�� true��� ���� ��¥�� ����� clearedDate�� ���ؼ� Ŭ���� �������� ����
    }

    public void Save()
    {
        // Quest�� Current, Cleared, Executable ����
    }

    public void Perform(string id)
    {
        // NOTE :
        // Quest�� Index�� ��� ��ȯ�ϴ� Ȯ��޼ҵ� �ۼ�

        Quest quest = DailyQuests.Find((quest) => quest.Id.Equals(id));
        if (quest != null) quest.Perform();

        quest = RepeatableQuests.Find((quest) => quest.Id.Equals(id));
        if (quest != null) quest.Perform();
    }
}
