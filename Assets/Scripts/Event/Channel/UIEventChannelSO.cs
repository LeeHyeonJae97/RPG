using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UIEventChannel", menuName = "ScriptableObject/Event/UIEventChannel")]
public class UIEventChannelSO : BaseEventChannelSO
{
    public UnityAction openCharacter;
    public UnityAction openCorps;
    public UnityAction openInventory;
    public UnityAction openEquipment;
    public UnityAction openSkill;
    public UnityAction openRune;
}
