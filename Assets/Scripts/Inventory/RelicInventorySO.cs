using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE :
// �޸𸮿� ��� �÷����� ���� �ʿ䰡 ��������?

[CreateAssetMenu(fileName = "RelicInventory", menuName = "ScriptableObject/Inventory/RelicInventory")]
public class RelicInventorySO : ScriptableObject
{
    [field:SerializeField] public List<RelicSO> Items { get; protected set; } = new List<RelicSO>();

    public void Load()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            // Relic �̸��� ���� ���� �����ϰ� �ҷ��;��Ѵ�.
            Items[i].Init(10);
        }
    }

    public void Save()
    {

    }
}
