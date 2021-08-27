using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

[CreateAssetMenu(fileName = "CharacterInventory", menuName = "ScriptableObject/Inventory/CharacterInventory")]
public class CharacterInventorySO : BaseInventorySO<Character>
{
    public override void Load()
    {
        // ���ʿ��� ����
        CharacterStat[] stats = new CharacterStat[Variables.STAT_TYPE_COUNT];
        for (int i = 0; i < stats.Length; i++)
            stats[i] = new CharacterStat(Random.Range(5, 11));
        Character character = new Character("���谡", null, stats);
        character.Equipped(0, 0);
        Add(character);
    }

    public override void Save()
    {

    }
}
