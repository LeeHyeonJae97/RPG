using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneInventory", menuName = "ScriptableObject/Inventory/RuneInventory")]
public class RuneInventorySO : BaseInventorySO<Rune>
{
    public override void Load()
    {
        // 세이브 파일 로드
    }

    public override void Save()
    {

    }
}
