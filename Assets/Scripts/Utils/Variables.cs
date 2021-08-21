using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainTab { Combat, Character, Inventory, Smithy, Dungeon, Shop }
public enum InventoryTab { Equipment, Skill, Rune, Relic }
public enum SmithyTab { Introduce, Make, Upgrade, Enchant, Disassemble }
public enum UserStat { Gold, ForUpgrade, ForEnchant, Exp, EquipmentDrop, BossAppearance, GameSpeed}

public static class Variables
{
    public const string FILE_USERDATA = "UserData";
    public const string FILE_CORPS = "Corps";
    public const string FILE_CHARACTER = "Character";
    public const string FILE_EQUIPMENT = "Equipment";
    public const string FILE_SKILL = "Skill";
    public const string FILE_RUNE = "Rune";
    public const string FILE_RELIC = "Relic";
    public const string FILE_QUEST = "Quest";
    public const string FILE_PASSWORD = "HELLOCWORLD";

    public const int COST_INTRODUCE_CHARACTER = 100;
    public const int COST_MAKE_EQUIPMENT = 100;
    public const int COST_MAKE_SKILL = 100;
    public const int COST_MAKE_RUNE = 100;
    public const int COST_ENCHANT = 200;
    public const int COST_UPGRADE = 100;
    public const int COST_DISASSEMBLE = 1000;

    public static float[] UpgradeSuccessPercent = { 80, 80, 60, 40, 30, 15, 10, 5, 3, 1, 0.3f, 0.2f, 0.1f };
    public static int[] MaxExps = { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    public const string STAT_MAXHP = "�ִ�ü��";
    public const string STAT_PHYSICAL_ATK = "�������ݷ�";
    public const string STAT_MAGICAL_ATK = "�������ݷ�";
    public const string STAT_PHYSICAL_DEF = "��������";
    public const string STAT_MAGICAL_DEF = "��������";
    public const string STAT_CRITICAL_ATK = "ũ��Ƽ�ð��ݷ�";
    public const string STAT_CRITICAL_PERCENT = "ũ��Ƽ��Ȯ��";
    public const string STAT_REDUCE_COOLDOWN = "��Ÿ�Ӱ���";
    public const string STAT_RESISTANCE_CC = "�����̻�����";

    public static string[] StatNames =
    {
        STAT_MAXHP,
        STAT_PHYSICAL_ATK,
        STAT_MAGICAL_ATK,
        STAT_PHYSICAL_DEF,
        STAT_MAGICAL_DEF,
        STAT_CRITICAL_ATK,
        STAT_CRITICAL_PERCENT,
        STAT_REDUCE_COOLDOWN,
        STAT_RESISTANCE_CC,
    };
}
