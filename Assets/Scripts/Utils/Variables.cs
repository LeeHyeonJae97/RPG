using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { Hp, PhyAtk, MagAtk, PhyDef, MagDef, CrtAtk, CrtPrc, CdRdc, CcPst }
public enum UserStatType { Gold, ForUpgrade, ForEnchant, Exp, EquipmentDrop, Boss, GameSpeed }

public enum MainTab { Combat, Character, Inventory, Smithy, Dungeon, Shop }
public enum InventoryTab { Equipment, Skill, Rune, Relic }
public enum SmithyTab { Introduce, Make, Upgrade, Enchant, Disassemble }
public enum UserStat { Gold, ForUpgrade, ForEnchant, Exp, EquipmentDrop, BossAppearance, GameSpeed}

public static class Variables
{
    public const int STAT_TYPE_COUNT = 9;    

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
}
