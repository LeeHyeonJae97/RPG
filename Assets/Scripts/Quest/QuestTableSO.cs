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
        // 세이브된 정보 로드

        // NOTE :
        // Daily는 cleared가 true라면 오늘 날짜와 저장된 clearedDate를 비교해서 클리어 가능한지 결정
    }

    public void Save()
    {
        // Quest의 Current, Cleared, Executable 저장
    }
}
