using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Corps", menuName = "ScriptableObject/Corps")]
public class CorpsSO : ScriptableObject
{
    public int joinedPresetIndex;

    public Preset[] Presets { get; private set; } = new Preset[5];

    public bool Joinable(int presetIndex)
    {
        if (presetIndex == joinedPresetIndex) return false;

        Combatant[] combatants = Presets[presetIndex].Combatants;
        return combatants[0].IsCharacterEquipped || combatants[1].IsCharacterEquipped || combatants[2].IsCharacterEquipped;
    }

    public bool UnJoinable(int presetIndex, int combatPositionIndex)
    {
        if (presetIndex != joinedPresetIndex) return true;

        Combatant[] combatants = Presets[presetIndex].Combatants;
        for (int i = 0; i < combatants.Length; i++)
        {
            if (i != combatPositionIndex && combatants[i].IsCharacterEquipped)
                return true;
        }

        return false;
    }

    private void OnEnable()
    {
        for (int i = 0; i < Presets.Length; i++)
        {
            Presets[i] = new Preset();
            Combatant[] combatants = Presets[i].Combatants;
            for (int j = 0; j < combatants.Length; j++)
            {
                if (combatants[j] == null)
                    combatants[j] = new Combatant();
            }
        }
    }

    public void Load(int joinedPresetIndex, List<Character> characters, List<Equipment> equipments, List<Skill> skills)
    {
        this.joinedPresetIndex = joinedPresetIndex;

        for (int i = 0; i < characters.Count; i++)
        {
            for (int j = 0; j < characters[i].CombatPositionIndices.Length; j++)
            {
                int combatPosition = characters[i].CombatPositionIndices[j];
                if (combatPosition != -1)
                    Presets[j].Combatants[combatPosition].EquipCharacter(characters[i]);
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

[System.Serializable]
public class Preset
{
    [field: SerializeField] public Combatant[] Combatants { get; private set; } = new Combatant[3];
}
