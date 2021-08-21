using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

[CreateAssetMenu(fileName = "CharacterInventory", menuName = "ScriptableObject/Inventory/CharacterInventory")]
public class CharacterInventorySO : BaseInventorySO<Character>
{
    public override void Load()
    {
        // 최초에만 실행
        CharacterStat[] stats = new CharacterStat[Variables.StatNames.Length];
        for (int i = 0; i < stats.Length; i++)
            stats[i] = new CharacterStat(Variables.StatNames[i], Random.Range(3, 10));

        Character character = new Character("모험가", stats);
        character.Equipped(0, 0);
        Items.Add(character);
    }

    public override void Save()
    {

    }
}
