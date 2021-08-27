using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroducedCharacter
{
    public string characterName;
    public Sprite preview;
    public CharacterStat[] stats = new CharacterStat[9];

    public IntroducedCharacter()
    {
        for (int i = 0; i < stats.Length; i++)
            stats[i] = new CharacterStat(5);
    }

    public Character Conclude(string name, Sprite preview)
    {
        return new Character(name, preview, stats);
    }

    public void RandomStat()
    {

    }
}
