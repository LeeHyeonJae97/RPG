using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum CombatPosition { Invalid = -1, Front = 0, Middle = 1, Back = 2 }
public enum CombatTarget { All, Frontest, Backest, HighestHp }

public class CombatManager : MonoBehaviour
{
    [SerializeField] private LiveCombatant[] _liveCombatants = new LiveCombatant[3];
    [SerializeField] private LiveMonster[] _liveMonsters = new LiveMonster[3]; 

    private List<ILive> _targets = new List<ILive>();

    [SerializeField] private StatusManager _statusManager;

    [SerializeField] private UserDataSO _userData;
    [SerializeField] private RelicSO _goldRelic;
    [SerializeField] private RelicSO _forUpgradeRelic;
    [SerializeField] private RelicSO _forEnchantRelic;
    [SerializeField] private RelicSO _expRelic;

    [SerializeField] private UIEventChannelSO _channel;
    [SerializeField] private CombatEventChannelSO _combatEventChannel;
    [SerializeField] private StringEventChannelSO _alertModalEventChannel;
    [SerializeField] private QuestEventChannelSO _questEventChannel;

    private StageSO _stage;
    private Dictionary<int, MonsterSO> _stageMonsterDic;
    private int _curWaveIndex;

    private bool CombatantsAllDead
    {
        get
        {
            for (int i = 0; i < _liveCombatants.Length; i++)
            {
                if (!_liveCombatants[i].IsDead)
                    return false;
            }

            return true;
        }
    }
    private bool MonstersAllDead
    {
        get
        {
            for (int i = 0; i < _liveMonsters.Length; i++)
            {
                if (!_liveMonsters[i].IsDead)
                    return false;
            }

            return true;
        }
    }

    public StageSO InitStage(StageSO stage, MonsterSO[] monsters)
    {
        StageSO old = _stage;

        _stage = stage;
        _stageMonsterDic = new Dictionary<int, MonsterSO>();
        for (int i = 0; i < monsters.Length; i++)
            _stageMonsterDic.Add(monsters[i].Id, monsters[i]);
        _curWaveIndex = 0;

        Joined(_stage.Waves[_curWaveIndex]);

        return old;
    }

    public void RestartStage()
    {
        _curWaveIndex = 0;
        Joined(_stage.Waves[_curWaveIndex]);
    }

    public void NextWave()
    {
        Joined(_stage.Waves[++_curWaveIndex]);
    }

    public void Joined(Combatant[] combatants)
    {       
        // 기존의 Combatant 해제
        for (int i = 0; i < _liveCombatants.Length; i++)
        {
            if (!_liveCombatants[i].IsEmpty)
            {
                _liveCombatants[i].Combatant.UnjoinCombat();
                _liveCombatants[i].Reset();
            }
        }

        // 새로운 Combatant 등록
        for (int i = 0; i < combatants.Length; i++)
        {
            if (combatants[i].IsCharacterEquipped)
            {
                combatants[i].JoinCombat((CombatPosition)i);
                _liveCombatants[i].Init(combatants[i], GetTarget, Die);
            }
        }
    }

    public void Joined(Wave wave)
    {
        MonsterSO[] monsters = new MonsterSO[wave.MonsterIds.Length];
        for (int i = 0; i < wave.MonsterIds.Length; i++)
            monsters[i] = _stageMonsterDic[wave.MonsterIds[i]];

        for (int i = 0; i < monsters.Length; i++)
            _liveMonsters[i].Init(monsters[i], GetTarget, Die);
    }

    private void Die(Combatant dead)
    {
        if (CombatantsAllDead)
        {
            for (int i = 0; i < _liveCombatants.Length; i++)
                _liveCombatants[i].Init();

            RestartStage();
        }
    }

    private void Die(MonsterSO dead)
    {
        int amount = (int)(dead.OutputExpRange.Random() * (1 + _expRelic.Buff.Value / 100));
        for (int i = 0; i < _liveCombatants.Length; i++)
        {
            if (!_liveCombatants[i].IsEmpty)
                _liveCombatants[i].Combatant.Character.Exp += amount;
        }

        // NOTE :
        // UI 업데이트
        _channel.updateCombatingCharacterInfoUI?.Invoke();

        // NOTE :
        // MoneyType에서의 값이 UserStat에서의 값과 동일하기 때문에 RelicInventory의 Items에서 MoneyType값으로 맞는 Relic을 찾을 수 있다.

        switch (dead.OutputMoneyType)
        {
            case MoneyType.Gold:
                _statusManager.EarnMoney(MoneyType.Gold, (int)(dead.OutputMoneyRange.Random() * (1 + _goldRelic.Buff.Value / 100)));
                break;
            case MoneyType.ForUpgrade:
                _statusManager.EarnMoney(MoneyType.ForUpgrade, (int)(dead.OutputMoneyRange.Random() * (1 + _forUpgradeRelic.Buff.Value / 100)));
                break;
            case MoneyType.ForEnchant:
                _statusManager.EarnMoney(MoneyType.ForEnchant, (int)(dead.OutputMoneyRange.Random() * (1 + _forEnchantRelic.Buff.Value / 100)));
                break;
        }

        _questEventChannel.perform?.Invoke("Kill");

        if (MonstersAllDead)
        {
            if (_curWaveIndex < _stage.Waves.Length - 1)
                NextWave();
            else
                RestartStage();
        }
    }

    public MonsterSO testMonster;

    [Button("Test Die")]
    public void TestDie()
    {
        Die(testMonster);
    }

    #region GetTarget

    public List<ILive> GetTarget(string tag, CombatTarget type, int count)
    {
        ILive[] candidates = null;
        switch (tag)
        {
            case "Combatant":
                candidates = _liveCombatants;
                break;

            case "Monster":
                candidates = _liveMonsters;
                break;
        }

        switch (type)
        {
            case CombatTarget.Frontest:
                return Frontest(candidates, count);

            case CombatTarget.Backest:
                return Backest(candidates, count);

            case CombatTarget.HighestHp:
                return HighestHp(count);

            case CombatTarget.All:
                return All(candidates);

            default:
                return null;
        }
    }

    public List<ILive> Frontest(ILive[] candidates, int count)
    {
        _targets.Clear();

        for (int i = 0; i < candidates.Length; i++)
        {
            if (!candidates[i].IsEmpty && !candidates[i].IsDead)
                _targets.Add(candidates[i]);
        }

        return _targets;
    }

    public List<ILive> Backest(ILive[] candidates, int count)
    {
        _targets.Clear();

        for (int i = candidates.Length - 1; i >= 0; i--)
        {
            if (!candidates[i].IsEmpty && !candidates[i].IsDead)
                _targets.Add(candidates[i]);
        }

        return _targets;
    }

    public List<ILive> HighestHp(int count)
    {
        return _targets;
    }

    public List<ILive> All(ILive[] candidates)
    {
        _targets.Clear();
        _targets.AddRange(candidates);

        return _targets;
    }

    #endregion
}
