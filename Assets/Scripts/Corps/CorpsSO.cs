using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

[CreateAssetMenu(fileName = "Corps", menuName = "ScriptableObject/Corps")]
public class CorpsSO : ScriptableObject
{
    public int joinedPresetIndex;

    public Combatant[,] Combatants { get; private set; } = new Combatant[5, 3];

    public Combatant[] GetCombatants(int presetIndex)
    {
        return new Combatant[] { Combatants[presetIndex, 0], Combatants[presetIndex, 1], Combatants[presetIndex, 2] };
    }

    public void Load(int joinedPresetIndex, List<Character> characters, List<Equipment> equipments, List<Skill> skills)
    {
        this.joinedPresetIndex = joinedPresetIndex;

        for (int i = 0; i < Combatants.GetLength(0); i++)
        {
            for (int j = 0; j < Combatants.GetLength(1); j++)
                Combatants[i, j] = new Combatant();
        }

        for (int i = 0; i < characters.Count; i++)
        {
            for (int j = 0; j < characters[i].CombatPositionIndices.Length; j++)
            {
                int combatPosition = characters[i].CombatPositionIndices[j];
                if (combatPosition != -1)
                    Combatants[j, combatPosition].EquipCharacter(characters[i]);
            }
        }

        //for (int i = 0; i < equipments.Count; i++)
        //{
        //    for (int j = 0; j < equipments[i].CombatPositionIndices.Length; j++)
        //        Combatants[j, equipments[i].CombatPositionIndices[j]].EquipEquipment(equipments[i].SlotIndices[j], equipments[i]);
        //}

        //for (int i = 0; i < skills.Count; i++)
        //{
        //    for (int j = 0; j < equipments[i].CombatPositionIndices.Length; j++)
        //        Combatants[j, skills[i].CombatPositionIndices[j]].EquipSkill(skills[i].SlotIndices[j], skills[i]);
        //}
    }
}
