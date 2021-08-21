using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Weapon, Armor, Accessory }

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObject/Item/Equipment")]
public class EquipmentSO : ItemSO
{
    [field: SerializeField] public EquipmentType Type { get; private set; }
    [field: SerializeField] public EquipmentBuffInfo[] Buffs { get; private set; }
    [field: SerializeField] public int MaxEnchantableCount { get; private set; }
}
