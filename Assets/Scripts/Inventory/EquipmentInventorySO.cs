using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "EquipmentInventory", menuName = "ScriptableObject/Inventory/EquipmentInventory")]
public class EquipmentInventorySO : BaseInventorySO<Equipment>
{
    public override void Load()
    {
        // 세이브 파일 로드
    }

    public override void Save()
    {

    }
}
