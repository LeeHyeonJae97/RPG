using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SkillInventory", menuName = "ScriptableObject/Inventory/SkillInventory")]
public class SkillInventorySO : BaseInventorySO<Skill>
{
    public override void Load()
    {
        // 세이브 파일 로드
    }

    public override void Save()
    {

    }
}
