using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class DailyQuest : Quest
{
    public string ClearedDate { get; private set; }
    public override bool Performable => string.IsNullOrEmpty(ClearedDate) || !ClearedDate.Equals(DateTime.Today.ToString("yyyyMMdd"));

    public DailyQuest(string questName, int required, int reward)
    {
        Id = questName;
        Required = required;
        Reward = reward;
    }

    public override void ReceiveReward()
    {
        if (Clearable)
        {
            Current = 0;
            ClearedDate = DateTime.Today.ToString("yyyyMMdd");
        }
    }
}
