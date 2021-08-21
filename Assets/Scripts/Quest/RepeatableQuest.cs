using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RepeatableQuest : Quest
{
    public override bool Performable => true;

    public override void ReceiveReward()
    {
        if (Clearable) Current -= Required;
    }
}
