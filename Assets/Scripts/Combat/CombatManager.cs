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

    private List<Live> _targets = new List<Live>();

    [SerializeField] private CorpsSO _corps;
    [SerializeField] private UserDataSO _userData;
    [SerializeField] private RelicSO _goldRelic;
    [SerializeField] private RelicSO _forUpgradeRelic;
    [SerializeField] private RelicSO _forEnchantRelic;
    [SerializeField] private RelicSO _expRelic;

    [SerializeField] private CombatEventChannelSO _combatEventChannel;
    [SerializeField] private QuestEventChannelSO _questEventChannel;
    [SerializeField] private StageEventChannelSO _stageEventChannel;

    private StageSO _stage;
    private int _curWaveIndex;

    private bool CombatantsAllDead
    {
        get
        {
            for (int i = 0; i < _liveCombatants.Length; i++)
            {
                if (!_liveCombatants[i].IsEmpty && !_liveCombatants[i].IsDead)
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
                if (!_liveMonsters[i].IsEmpty && !_liveMonsters[i].IsDead)
                    return false;
            }

            return true;
        }
    }

    private void Start()
    {
        _stageEventChannel.startStage += StartStage;
    }

    private void OnDestroy()
    {
        _stageEventChannel.startStage -= StartStage;
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////

    public void StartStage(StageSO stage)
    {
        Debug.Log("Start Stage");

        if (_stage != null && _stage != stage) Addressables.Release(_stage);

        _stage = stage;
        _curWaveIndex = -1;
        NextWave();
    }

    private void NextWave()
    {
        Debug.Log("Next Wave");
        StartCoroutine(NextWaveCoroutine());
    }

    private IEnumerator NextWaveCoroutine()
    {
        Pause(true);
        yield return new WaitForSeconds(1);


        _curWaveIndex++;
        if (_curWaveIndex >= _stage.Waves.Length)
        {
            _curWaveIndex = 0;
            CombatantRejoined();
        }
        MonsterJoined(_stage.Waves[_curWaveIndex].Monsters);

        Pause(false);
    }

    public void CombatantJoined()
    {
        CombatantJoined(_corps.joinedPresetIndex);
    }

    public void CombatantJoined(int presetIndex)
    {
        _corps.joinedPresetIndex = presetIndex;

        Combatant[] combatants = _corps.Presets[presetIndex].Combatants;

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

    public void CombatantRejoined()
    {
        for (int i = 0; i < _liveCombatants.Length; i++)
        {
            if (!_liveCombatants[i].IsEmpty)
                _liveCombatants[i].Init();
        }
    }

    public void MonsterJoined(MonsterSO[] monsters)
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] != null)
                _liveMonsters[i].Init(monsters[i], GetTarget, Die);
        }
    }

    public void MonsterRejoined()
    {
        for (int i = 0; i < _liveMonsters.Length; i++)
        {
            if (!_liveMonsters[i].IsEmpty)
                _liveMonsters[i].Init();
        }
    }

    /////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////

    private void Die(Combatant dead)
    {
        //if (CombatantsAllDead) RestartStage();
    }

    private void Die(MonsterSO dead)
    {
        // 경험치
        int amount = (int)(dead.OutputExpRange.Random() * (1 + _expRelic.Buff.Value / 100));
        for (int i = 0; i < _liveCombatants.Length; i++)
        {
            if (!_liveCombatants[i].IsEmpty)
                _liveCombatants[i].Combatant.Character.Exp += amount;
        }

        // 재화
        switch (dead.OutputMoneyType)
        {
            case MoneyType.Gold:
                _userData.EarnMoney(MoneyType.Gold, (int)(dead.OutputMoneyRange.Random() * (1 + _goldRelic.Buff.Value / 100)));
                break;
            case MoneyType.ForUpgrade:
                _userData.EarnMoney(MoneyType.ForUpgrade, (int)(dead.OutputMoneyRange.Random() * (1 + _forUpgradeRelic.Buff.Value / 100)));
                break;
            case MoneyType.ForEnchant:
                _userData.EarnMoney(MoneyType.ForEnchant, (int)(dead.OutputMoneyRange.Random() * (1 + _forEnchantRelic.Buff.Value / 100)));
                break;
        }

        // 퀘스트
        _questEventChannel.perform?.Invoke("Kill");

        // 클리어 체크
        if (MonstersAllDead) NextWave();
    }

    private void Pause(bool value)
    {
        for (int i = 0; i < _liveCombatants.Length; i++)
            _liveCombatants[i].enabled = !value;
        for (int i = 0; i < _liveMonsters.Length; i++)
            _liveMonsters[i].enabled = !value;
    }

    #region GetTarget

    public List<Live> GetTarget(string tag, CombatTarget type, int count)
    {
        Live[] candidates = null;
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

    public List<Live> Frontest(Live[] candidates, int count)
    {
        _targets.Clear();

        for (int i = 0; i < candidates.Length; i++)
        {
            if (!candidates[i].IsEmpty && !candidates[i].IsDead)
                _targets.Add(candidates[i]);
        }

        return _targets;
    }

    public List<Live> Backest(Live[] candidates, int count)
    {
        _targets.Clear();

        for (int i = candidates.Length - 1; i >= 0; i--)
        {
            if (!candidates[i].IsEmpty && !candidates[i].IsDead)
                _targets.Add(candidates[i]);
        }

        return _targets;
    }

    public List<Live> HighestHp(int count)
    {
        return _targets;
    }

    public List<Live> All(Live[] candidates)
    {
        _targets.Clear();
        _targets.AddRange(candidates);

        return _targets;
    }

    #endregion
}
