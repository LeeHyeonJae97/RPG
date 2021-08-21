using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoneyType { Gold, ForUpgrade, ForEnchant, Diamond }

[CreateAssetMenu(fileName = "Money", menuName = "ScriptableObject/Money")]
public class MoneySO : ScriptableObject
{
    [field: SerializeField] public string MoneyName { get; private set; }
    [field: SerializeField] public MoneyType Type { get; private set; }
    [HideInInspector] public int amount;
    [field: SerializeField] public Sprite Icon { get; private set; }
}
