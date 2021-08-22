using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObject/UserData")]
public class UserDataSO : ScriptableObject
{
    [field: SerializeField] public string Nickname { get; set; }
    public int stage;
    [field: SerializeField] public MoneySO[] Moneys { get; private set; } = new MoneySO[5];

    public void Load()
    {
        // 세이브 파일 로드

        for (int i = 0; i < Moneys.Length; i++)
            Moneys[i].amount = 9999;
    }
}
