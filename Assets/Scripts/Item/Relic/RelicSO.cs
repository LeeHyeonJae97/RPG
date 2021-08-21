using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Relic", menuName = "ScriptableObject/Item/Relic")]
public class RelicSO : ScriptableObject
{
    // ����
    // 1. ��� ȹ�淮
    // 2. ��ȭ��ȭ ȹ�淮
    // 3. ������ȭ ȹ�淮
    // 4. ���� ���� Ȯ��
    // 5. ��� �����
    // 6. ����ġ ȹ�淮
    // 7. ���� �ӵ�

    // NOTE :
    // ��带 ȹ���ϴ� �κ�(ex. MonsterSO.Die())���� ȹ���ϰԵǴ� ��ȭ�� ���� RelicSO�� ���� ���� ����ؼ� MoneySO�� amount�� ����

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
