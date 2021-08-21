using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSlot : BaseInventorySlot<Rune>
{
    public override void UpdateUI(Rune rune)
    {
        _previewImage.sprite = rune.Info.Preview;
    }
}
