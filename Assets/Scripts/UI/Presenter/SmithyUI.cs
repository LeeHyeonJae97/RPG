using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public enum UpgradeState
{
    None,
    EquipmentSelected,
    SkillSelected,
}

public enum DisassembleState
{
    None,
    EquipmentSelected,
    SkillSelected,
    RuneSelected,
}

public class SmithyUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterInventorySO _characterInventory;
    [SerializeField] private EquipmentInventorySO _equipmentInventory;
    [SerializeField] private SkillInventorySO _skillInventory;
    [SerializeField] private RuneInventorySO _runeInventory;
    [SerializeField] private UserDataSO _userData;

    [Header("Cutout Panel")]
    [SerializeField] private RectTransform _inventoryPanel;
    [SerializeField] private RectTransform _equipmentPanel;
    [SerializeField] private RectTransform _runePanel;

    public static UpgradeState UpgradeState;
    public static DisassembleState DisassembleState;

    public static Equipment SelectedEquipmentForUpgrade;
    public static Skill SelectedSkillForUpgrade;
    public static Equipment SelectedEquipmentForEnchant;
    public static Rune SelectedRuneForEnchant;
    public static Equipment SelectedEquipmentForDisassemble;
    public static Skill SelectedSkillForDisassemble;
    public static Rune SelectedRuneForDisassemble;

    public void Reset()
    {

    }

    #region Make

    [Header("Make")]
    [SerializeField] private Canvas _introducedCharacterCanvas;
    [SerializeField] private Canvas _madeItemListCanvas;
    [SerializeField] private InputField _characterNameInputField;
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI[] _statTexts;
    [SerializeField] private MadeItemSlot[] _slots;

    private IntroducedCharacter _introducedCharacter = new IntroducedCharacter();
    private AsyncOperationHandle<IList<Sprite>> _characterPreviewLoadHandle;
    private int _characterPreviewIndex;

    public void IntroduceCharacterButton()
    {
        StartCoroutine(IntroduceCharacterCoroutine());
    }

    public IEnumerator IntroduceCharacterCoroutine()
    {
        if (!_userData.Affordable(MoneyType.Diamond, Variables.COST_INTRODUCE_CHARACTER)) yield break;

        _userData.LoseMoney(MoneyType.Diamond, Variables.COST_INTRODUCE_CHARACTER);

        // ĳ���� ����
        _characterPreviewLoadHandle = Addressables.LoadAssetsAsync<Sprite>("CharacterPreview", null);
        yield return new WaitUntil(() => _characterPreviewLoadHandle.IsDone);

        _characterPreviewIndex = 0;

        OpenIntroduceCharacterUI();
    }

    public void Reintroduce()
    {
        _introducedCharacter.RandomStat();
        UpdateIntroduceCharacterUI();
    }

    public void SwitchCharacterPreview(bool left)
    {
        _characterPreviewIndex = left ? _characterPreviewIndex - 1 : _characterPreviewIndex + 1;
        _characterPreviewIndex = Mathf.Clamp(_characterPreviewIndex, 0, _characterPreviewLoadHandle.Result.Count - 1);
        UpdateIntroduceCharacterUI();
    }

    public void Hire()
    {
        _characterInventory.Add(_introducedCharacter.Conclude("", null));
        Addressables.Release(_characterPreviewLoadHandle);

        _introducedCharacterCanvas.enabled = false;
    }

    private void OpenIntroduceCharacterUI()
    {
        UpdateIntroduceCharacterUI();
        _introducedCharacterCanvas.enabled = true;        
    }

    private void UpdateIntroduceCharacterUI()
    {
        _previewImage.sprite = _introducedCharacter.preview;

        for (int i = 0; i < _statTexts.Length; i++)
            _statTexts[i].text = $"{(StatType)i}    {_introducedCharacter.stats[i].Value}";
    }

    public void MakeEquipments(int count)
    {
        if (!_userData.Affordable(MoneyType.Diamond, Variables.COST_MAKE_EQUIPMENT)) return;

        _userData.LoseMoney(MoneyType.Diamond, Variables.COST_MAKE_EQUIPMENT);
        StartCoroutine(MakeEquipmentsCoroutine(count));
    }

    public IEnumerator MakeEquipmentsCoroutine(int count)
    {
        Waiting.Begin?.Invoke("��� ����...");

        AsyncOperationHandle<IList<EquipmentSO>> handle = Addressables.LoadAssetsAsync<EquipmentSO>("Equipments", null);
        yield return new WaitUntil(() => handle.IsDone);

        List<EquipmentSO> infos = handle.Result.ToList();

        // NOTE :
        // Ȯ���� ���� �������� count ��ŭ �����ϵ��� ����
        EquipmentSO[] picks = new EquipmentSO[count];
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, infos.Count);
            AsyncOperationHandle<EquipmentSO> handle2 = Addressables.LoadAssetAsync<EquipmentSO>($"Equipment_{infos[index].Id}");
            yield return new WaitUntil(() => handle2.IsDone);

            picks[i] = handle2.Result;
            _equipmentInventory.Add(new Equipment(picks[i]));
        }
        Addressables.Release(handle);

        Waiting.End?.Invoke();

        // ������ Equipment�� UI�� ǥ���ϰ� Inventory�� �߰�
        OpenMadeItemListUI(picks);
    }

    public void MakeSkills(int count)
    {
        if (!_userData.Affordable(MoneyType.Diamond, Variables.COST_MAKE_SKILL)) return;

        _userData.LoseMoney(MoneyType.Diamond, Variables.COST_MAKE_SKILL);
        StartCoroutine(MakeSkillsCoroutine(count));
    }

    public void MakeRunes(int count)
    {
        if (!_userData.Affordable(MoneyType.Diamond, Variables.COST_MAKE_RUNE)) return;

        _userData.LoseMoney(MoneyType.Diamond, Variables.COST_MAKE_RUNE);
        StartCoroutine(MakeRunesCoroutine(count));
    }

    private IEnumerator MakeSkillsCoroutine(int count)
    {
        Waiting.Begin?.Invoke("");

        AsyncOperationHandle<IList<SkillSO>> handle = Addressables.LoadAssetsAsync<SkillSO>("Skills", null);
        yield return new WaitUntil(() => handle.IsDone);

        List<SkillSO> infos = handle.Result.ToList();

        // NOTE :        
        // Ȯ���� ���� �������� count��ŭ �����ϵ��� ����
        SkillSO[] picks = new SkillSO[count];
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, infos.Count);
            AsyncOperationHandle<SkillSO> handle2 = Addressables.LoadAssetAsync<SkillSO>($"Skill_{infos[index].Id}");
            yield return new WaitUntil(() => handle2.IsDone);

            picks[i] = handle2.Result;
            _skillInventory.Add(new Skill(picks[i]));
        }
        Addressables.Release(handle);

        Waiting.End?.Invoke();

        // ������ Equipment�� UI�� ǥ���ϰ� Inventory�� �߰�
        OpenMadeItemListUI(picks);
    }

    private IEnumerator MakeRunesCoroutine(int count)
    {
        Waiting.Begin?.Invoke("");

        AsyncOperationHandle<IList<RuneSO>> handle = Addressables.LoadAssetsAsync<RuneSO>("Runes", null);
        yield return new WaitUntil(() => handle.IsDone);

        List<RuneSO> infos = handle.Result.ToList();

        RuneSO[] picks = new RuneSO[count];
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, infos.Count);
            AsyncOperationHandle<RuneSO> handle2 = Addressables.LoadAssetAsync<RuneSO>($"Rune_{infos[index].Id}");
            yield return new WaitUntil(() => handle2.IsDone);

            picks[i] = handle2.Result;
            _runeInventory.Add(new Rune(picks[i]));
        }
        Addressables.Release(handle);

        Waiting.End?.Invoke();

        OpenMadeItemListUI(picks);
    }

    private void OpenMadeItemListUI(ItemSO[] items)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < items.Length)
            {
                int index = i;
                _slots[i].Init(items[index]);
            }
            else
            {
                _slots[i].gameObject.SetActive(false);
            }
        }

        _madeItemListCanvas.enabled = true;
    }
    
    public void CloseMadeItemListUI()
    {
        _madeItemListCanvas.enabled = false;
    }

    #endregion

    #region Upgrade

    [Header("Upgrade")]
    [SerializeField] private Image _upgradePreviewImage;
    [SerializeField] private TextMeshProUGUI _upgradeCostText;
    [SerializeField] private TextMeshProUGUI _upgradeSuccessPercentText;
    [SerializeField] private TextMeshProUGUI _upgradeBuffsText;

    public void ClickUpgradeSlot()
    {
        if (UpgradeState != UpgradeState.None)
        {
            switch (UpgradeState)
            {
                case UpgradeState.EquipmentSelected:
                    SelectedEquipmentForUpgrade = null;
                    UpdateUpgradeEquipmentInfoUI();
                    break;

                case UpgradeState.SkillSelected:
                    SelectedSkillForUpgrade = null;
                    UpdateUpgradeSkillInfoUI();
                    break;
            }
            UpgradeState = UpgradeState.None;
        }
        else
        {
            CorpsUI.State = State.ClickUpgradeSlot;
            Cutout.Do(_inventoryPanel);
            Cutout.OnCanceled += () => CorpsUI.State = State.Idle;
        }
    }

    public void UpdateUpgradeInfoUI()
    {
        switch (UpgradeState)
        {
            case UpgradeState.None:
                UpdateUpgradeEquipmentInfoUI();
                break;

            case UpgradeState.EquipmentSelected:
                UpdateUpgradeEquipmentInfoUI();
                break;

            case UpgradeState.SkillSelected:
                UpdateUpgradeSkillInfoUI();
                break;
        }
    }

    private void UpdateUpgradeEquipmentInfoUI()
    {
        if (SelectedEquipmentForUpgrade == null)
        {
            _previewImage.gameObject.SetActive(false);
            _enchantSuccessPercentText.text = "----";
            _enchantCostText.text = "----";
            _upgradeBuffsText.text = "----";
        }
        else
        {
            _previewImage.sprite = SelectedEquipmentForUpgrade.Info.Preview;
            _previewImage.gameObject.SetActive(true);
            _enchantSuccessPercentText.text = "����Ȯ��";
            _enchantCostText.text = "���";
            _upgradeBuffsText.text = SelectedEquipmentForUpgrade.BuffsDescriptionWithNextLevel;
        }
    }

    private void UpdateUpgradeSkillInfoUI()
    {
        _previewImage.sprite = SelectedSkillForUpgrade.Info.Preview;
        _enchantSuccessPercentText.text = "����Ȯ��";
        _enchantCostText.text = "���";
        _upgradeBuffsText.text = SelectedSkillForUpgrade.DescriptionWithNextLevel;
    }

    public void Upgrade()
    {
        if (UpgradeState == UpgradeState.None)
        {
            AlertModal.Do?.Invoke("��ȭ�� ���/��ų�� ������ �ּ���");
            return;
        }
        else
        {
            if (!_userData.Affordable(MoneyType.ForUpgrade, Variables.COST_UPGRADE)) return;

            _userData.LoseMoney(MoneyType.ForUpgrade, Variables.COST_UPGRADE);

            bool success;

            switch (UpgradeState)
            {
                case UpgradeState.EquipmentSelected:
                    success = SelectedEquipmentForUpgrade.Upgraded();
                    break;

                case UpgradeState.SkillSelected:
                    success = SelectedSkillForUpgrade.Upgraded();
                    break;

                default:
                    Debug.LogError("Error");
                    return;
            }
            AlertModal.Do?.Invoke(success ? "��ȭ ����!" : "��ȭ ����!");
        }
    }

    #endregion

    #region Enchant

    [Header("Enchant")]
    [SerializeField] private Image _enchantEquipmentPreviewImage;
    [SerializeField] private TextMeshProUGUI _enchantableCountText;
    [SerializeField] private TextMeshProUGUI _enchantCostText;
    [SerializeField] private Image _enchantRunePreviewImage;
    [SerializeField] private TextMeshProUGUI _enchantSuccessPercentText;
    [SerializeField] private TextMeshProUGUI _runeBuffsText;

    public void ClickEnchantEquipmentSlot()
    {
        if (SelectedEquipmentForEnchant != null)
        {
            SelectedEquipmentForEnchant = null;
            UpdateEnchantEquipmentInfoUI();
        }
        else
        {
            CorpsUI.State = State.ClickEquipmentEnchantSlot;
            Cutout.Do(_equipmentPanel);
            Cutout.OnCanceled += () => CorpsUI.State = State.Idle;
        }
    }

    public void ClickEnchantRuneSlot()
    {
        if (SelectedRuneForEnchant != null)
        {
            SelectedRuneForEnchant = null;
            UpdateEnchantRuneInfoUI();
        }
        else
        {
            CorpsUI.State = State.ClickRuneEnchantSlot;
            Cutout.Do(_runePanel);
            Cutout.OnCanceled += () => CorpsUI.State = State.Idle;
        }
    }

    public void UpdateEnchantEquipmentInfoUI()
    {
        if (SelectedEquipmentForEnchant != null)
        {
            _enchantEquipmentPreviewImage.sprite = SelectedEquipmentForEnchant.Info.Preview;
            _enchantEquipmentPreviewImage.gameObject.SetActive(true);
            _enchantableCountText.text = $"���� ���� Ƚ�� {SelectedEquipmentForEnchant.EnchantableCount}";
        }
        else
        {
            _enchantEquipmentPreviewImage.gameObject.SetActive(false);
            _enchantableCountText.text = "----";
        }

        // NOTE :
        // ����� Equipment ����� Star�� ���� ����?
    }

    public void UpdateEnchantRuneInfoUI()
    {        
        if (SelectedRuneForEnchant != null)
        {
            _enchantRunePreviewImage.sprite = SelectedRuneForEnchant.Info.Preview;
            _enchantRunePreviewImage.gameObject.SetActive(true);
            _enchantSuccessPercentText.text = $"����Ȯ�� {SelectedRuneForEnchant.Info.EnchantSuccessPercent}%";
            _runeBuffsText.text = SelectedRuneForEnchant.BuffsDescription;
        }
        else
        {
            _enchantRunePreviewImage.gameObject.SetActive(false);
            _enchantSuccessPercentText.text = "----";
            _runeBuffsText.text = "----";
        }
    }

    public void Enchant()
    {
        if (SelectedEquipmentForEnchant == null)
        {
            AlertModal.Do?.Invoke("��� �������ּ���");
            return;
        }
        if (SelectedRuneForEnchant == null)
        {
            AlertModal.Do?.Invoke("���� �������ּ���");
            return;
        }
        if (!_userData.Affordable(MoneyType.ForEnchant, Variables.COST_ENCHANT)) return;

        Equipment equipment = SelectedEquipmentForEnchant;
        if (!equipment.Enchantable)
        {
            AlertModal.Do?.Invoke("�� �̻� ������ �Ұ����մϴ�.");
            return;
        }

        _userData.LoseMoney(MoneyType.ForEnchant, Variables.COST_ENCHANT);

        Rune rune = SelectedRuneForEnchant;

        bool success = equipment.Enchanted(rune);
        AlertModal.Do?.Invoke(success ? "���� ����!" : "���� ����!");

        _runeInventory.Remove(SelectedRuneForEnchant);
        SelectedRuneForEnchant = null;
        UpdateEnchantRuneInfoUI();
    }

    #endregion

    #region Disassemble

    [Header("Disassemble")]
    [SerializeField] private Image _disassemblePreviewImage;

    public void ClickDisassembleSlot()
    {
        if (DisassembleState != DisassembleState.None)
        {
            switch (DisassembleState)
            {
                case DisassembleState.EquipmentSelected:
                    SelectedEquipmentForDisassemble = null;
                    break;

                case DisassembleState.SkillSelected:
                    SelectedSkillForDisassemble = null;
                    break;

                case DisassembleState.RuneSelected:
                    SelectedRuneForDisassemble = null;
                    break;
            }
            DisassembleState = DisassembleState.None;
            UpdateDisassembleInfoUI();
        }
        else
        {
            CorpsUI.State = State.ClickDisassembleSlot;
            Cutout.Do(_inventoryPanel);
            Cutout.OnCanceled += () => CorpsUI.State = State.Idle;
        }
    }

    public void UpdateDisassembleInfoUI()
    {
        switch (DisassembleState)
        {
            case DisassembleState.None:
                UpdateDisassembleInfoUI(null);
                break;

            case DisassembleState.EquipmentSelected:
                UpdateDisassembleInfoUI(SelectedEquipmentForDisassemble.Info);
                break;

            case DisassembleState.SkillSelected:
                UpdateDisassembleInfoUI(SelectedSkillForDisassemble.Info);
                break;

            case DisassembleState.RuneSelected:
                UpdateDisassembleInfoUI(SelectedRuneForDisassemble.Info);
                break;

            default:
                Debug.LogError("Error");
                return;
        }
    }

    private void UpdateDisassembleInfoUI(ItemSO item)
    {
        if (item != null)
        {
            _disassemblePreviewImage.sprite = item.Preview;
            _disassemblePreviewImage.gameObject.SetActive(true);

            // ��ü ����� ����
        }
        else
        {
            _disassemblePreviewImage.gameObject.SetActive(false);
        }
    }

    public void Disassemble()
    {
        if (DisassembleState == DisassembleState.None)
        {
            AlertModal.Do?.Invoke("������ ���/��ų/����(��) ������ �ּ���");
            return;
        }
        else
        {
            if (!_userData.Affordable(MoneyType.Gold, Variables.COST_DISASSEMBLE)) return;

            _userData.LoseMoney(MoneyType.Gold, Variables.COST_DISASSEMBLE);

            int amount;

            switch (DisassembleState)
            {
                case DisassembleState.EquipmentSelected:
                    amount = SelectedEquipmentForDisassemble.Disassembled();
                    _equipmentInventory.Remove(SelectedEquipmentForDisassemble);
                    SelectedEquipmentForDisassemble = null;
                    break;

                case DisassembleState.SkillSelected:
                    amount = SelectedSkillForDisassemble.Disassembled();
                    _skillInventory.Remove(SelectedSkillForDisassemble);
                    SelectedSkillForDisassemble = null;
                    break;

                case DisassembleState.RuneSelected:
                    amount = SelectedRuneForDisassemble.Disassembled();
                    _runeInventory.Remove(SelectedRuneForDisassemble);
                    SelectedRuneForDisassemble = null;
                    break;

                default:
                    Debug.LogError("Error");
                    return;
            }
            DisassembleState = DisassembleState.None;

            _userData.EarnMoney(MoneyType.ForUpgrade, amount);

            UpdateDisassembleInfoUI();
        }
    }

    #endregion
}
