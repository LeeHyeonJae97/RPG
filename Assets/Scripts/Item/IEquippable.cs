using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable
{
    void Equipped(int presetIndex, int combatPositionIndex);
    void Equipped(int presetIndex, int combatPositionIndex, int slotIndex);
    void Released(int index);
}
