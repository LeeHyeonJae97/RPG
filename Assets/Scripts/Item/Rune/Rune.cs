using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : IDisasemblable
{
    public string Id { get; private set; }
    public RuneSO Info { get; private set; }
    public RuneBuff[] Buffs => Info.Buffs;

    public string BuffsDescription
    {
        get
        {
            string str = $"{Buffs[0].StatName} {Buffs[0].Value}%";
            for (int i = 1; i < Buffs.Length; i++)
            {
                str += "\n";
                str += $"{Buffs[i].StatName} {Buffs[i].Value}%";
            }

            return str;
        }
    }

    // »õ·Î¿î ·é È¹µæ
    public Rune(RuneSO info)
    {
        Id = "";
        Info = info;
    }

    // ±âÁ¸ÀÇ ·é ·Îµå
    public Rune(string id, RuneSO info)
    {
        Id = id;
        Info = info;
    }

    public int Disassembled()
    {
        return Info.DisassembleOutputAmountRange.Random();
    }
}
