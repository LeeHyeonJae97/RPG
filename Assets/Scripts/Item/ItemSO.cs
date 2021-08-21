using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Preview { get; private set; }
    [field: SerializeField] public int Star { get; private set; }
    [field: SerializeField] public int ResalePrice { get; private set; }
    [field: SerializeField] public Extension.RangeInt DisassembleOutputAmountRange { get; private set; }
}
