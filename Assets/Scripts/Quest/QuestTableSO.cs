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
}
