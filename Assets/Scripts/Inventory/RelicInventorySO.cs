using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE :
// 메모리에 계속 올려놓고 있을 필요가 없을지도?

[CreateAssetMenu(fileName = "RelicInventory", menuName = "ScriptableObject/Inventory/RelicInventory")]
public class RelicInventorySO : BaseInventorySO<RelicSO>
{
    public override void Load()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            // Relic 이름과 양을 따로 저장하고 불러와야한다.
            Items[i].Init(10);
        }
    }

    public override void Save()
    {

    }
}
