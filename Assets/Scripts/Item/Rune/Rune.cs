using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rune : IDisasemblable
{
    public string Id { get; private set; }
    public RuneSO Info { get; private set; }
    public RuneBuff[] Buffs => Info.Buffs;

    public string BuffsDescription
    {
        get
        {
            string str = $"{Buffs[0].Type} {Buffs[0].Value}%";
            for (int i = 1; i < Buffs.Length; i++)
            {
                str += "\n";
                str += $"{Buffs[i].Type} {Buffs[i].Value}%";
            }

            return str;
        }
    }

    public UnityAction<Rune> onValueChanged;
    public UnityAction onRemoved;

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
