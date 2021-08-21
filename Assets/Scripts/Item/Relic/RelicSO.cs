using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Relic", menuName = "ScriptableObject/Item/Relic")]
public class RelicSO : ScriptableObject
{
    // 버프
    // 1. 골드 획득량
    // 2. 강화재화 획득량
    // 3. 연성재화 획득량
    // 4. 보스 출현 확률
    // 5. 장비 드랍률
    // 6. 경험치 획득량
    // 7. 게임 속도

    // NOTE :
    // 골드를 획득하는 부분(ex. MonsterSO.Die())에서 획득하게되는 재화의 값에 RelicSO의 버프 값을 고려해서 MoneySO의 amount에 적용

    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public Sprite Preview { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int MaxAmount { get; private set; }
    public int Amount { get; private set; }
    [field: SerializeField] public RelicBuff Buff { get; private set; }

    public void Init(int amount)
    {
        Amount = amount;
        Buff.SetValue(amount);
    }

    public void Gain()
    {
        Buff.SetValue(++Amount);
    }
}
