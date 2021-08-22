using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Equipment : IEquippable, IUpgradable, IEnchantable, IDisasemblable
{
    [field:SerializeField] public string Id { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public EquipmentSO Info { get; private set; }
    [field: SerializeField] public EquipmentBuff[] Buffs { get; private set; }
    [field: SerializeField] public int EnchantableCount { get; private set; }
    [field: SerializeField] public List<RuneBuff> EnchantedBuffs { get; private set; }

    public bool IsEquipped
    {
        get
        {
            for (int i = 0; i < CombatPositionIndices.Length; i++)
            {
                if (CombatPositionIndices[i] != -1)
                    return true;
            }

            return false;
        }
    }
    public int[] CombatPositionIndices { get; private set; } = new int[5];
    public int[] SlotIndices { get; private set; } = new int[5];

    public bool Enchantable => EnchantableCount > 0;

    public string BuffsDescription
    {
        get
        {
            string str = $"{Buffs[0].Info.Type} {Buffs[0].Value}%";
            for (int i = 1; i < Buffs.Length; i++)
            {
                str += "\n";
                str += $"{Buffs[i].Info.Type} {Buffs[i].Value}%";
            }

            return str;
        }
    }

    public string BuffsDescriptionWithNextLevel
    {
        get
        {
            string str = $"{Buffs[0].Info.Type} {Buffs[0].Value} {Info.Buffs[0].GetValue(Level + 1)}";
            if (Buffs[0].Info.CalType == CalculationType.Percent) str += "%";
            for (int i = 1; i < Buffs.Length; i++)
            {
                str += "\n";
                str += $"{Buffs[i].Info.Type} {Buffs[i].Value} {Info.Buffs[0].GetValue(Level + 1)}";
                if (Buffs[i].Info.CalType == CalculationType.Percent) str += "%";
            }

            return str;
        }
    }

    public string EnchantedBuffsDescription
    {
        get
        {
            if (EnchantedBuffs.Count == 0) return "";

            string str = $"{EnchantedBuffs[0].Type} {EnchantedBuffs[0].Value}";
            if (EnchantedBuffs[0].CalType == CalculationType.Percent) str += "%";
            for (int i = 1; i < EnchantedBuffs.Count; i++)
            {
                str += "\n";
                str += $"{EnchantedBuffs[i].Type} {EnchantedBuffs[i].Value}";
                if (EnchantedBuffs[i].CalType == CalculationType.Percent) str += "%";
            }

            return str;
        }
    }

    // 새로운 장비 획득
    public Equipment(EquipmentSO info)
    {
        Id = "";
        Level = 1;
        Info = info;
        Buffs = new EquipmentBuff[Info.Buffs.Length];
        for (int i = 0; i < Buffs.Length; i++)
            Buffs[i] = new EquipmentBuff(info.Buffs[i], Level);
        EnchantableCount = info.MaxEnchantableCount;
        EnchantedBuffs = new List<RuneBuff>();

        Debug.Log(JsonUtility.ToJson(this));
    }

    // 기존의 장비 로드
    public Equipment(string id, int level, int enchantableCount, EquipmentSO info, List<RuneBuff> enchantedBuffs)
    {
        Id = id;
        Level = level;
        Info = info;
        Buffs = new EquipmentBuff[Info.Buffs.Length];
        for (int i = 0; i < Buffs.Length; i++)
            Buffs[i] = new EquipmentBuff(info.Buffs[i], Level);
        EnchantableCount = enchantableCount;
        EnchantedBuffs = enchantedBuffs;
    }

    public void Equipped(int presetIndex, int combatPositionIndex, int slotIndex)
    {
        CombatPositionIndices[presetIndex] = combatPositionIndex;
        SlotIndices[presetIndex] = slotIndex;
    }

    public void Released(int presetIndex)
    {
        CombatPositionIndices[presetIndex] = -1;
        SlotIndices[presetIndex] = -1;        
    }

    public bool Upgraded()
    {
        Level++;
        for (int i = 0; i < Buffs.Length; i++)
            Buffs[i].Upgrade(Info.Buffs[i].Formula, Level);

        return true;
    }

    public bool Enchanted(Rune rune)
    {
        EnchantableCount--;

        int random = Random.Range(0, 100);

        // Success
        if (random < rune.Info.EnchantSuccessPercent)
        {
            for (int i = 0; i < rune.Buffs.Length; i++)
                EnchantedBuffs.Add(new RuneBuff(rune.Buffs[i]));
            return true;
        }
        // Fail
        else
        {
            return false;
        }
    }

    public int Disassembled()
    {
        return Info.DisassembleOutputAmountRange.Random();
    }

    public void Equipped(int presetIndex, int combatPositionIndex) { }
}
