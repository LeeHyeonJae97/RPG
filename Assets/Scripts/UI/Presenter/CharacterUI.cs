using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    public static Character SelectedCharacter;

    [SerializeField] private CharacterInventorySO _inventory;
    [SerializeField] private CorpsSO _corps;

    [SerializeField] private CharacterSlot _slotPrefab;
    [SerializeField] private Transform _slotHolder;

    [SerializeField] private Canvas _infoCanvas;
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _statPointText;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private Image _expBarFillImage;
    [SerializeField] private TextMeshProUGUI[] _statTexts;
    [SerializeField] private Button[] _investButtons;
    [SerializeField] private Button _fireButton;

    [SerializeField] private RectTransform _corpsPanel;

    private void Start()
    {
        _inventory.onAdded += OnCharacterAdded;
        _inventory.onRemoved += OnCharacterRemoved;
    }

    private void OnDestroy()
    {
        _inventory.onAdded -= OnCharacterAdded;
        _inventory.onRemoved -= OnCharacterRemoved;
    }

    public void OnClickEquipButton()
    {
        Cutout.Do?.Invoke(_corpsPanel);
    }

    public void Release()
    {
        for (int i = 0; i < SelectedCharacter.CombatPositionIndices.Length; i++)
        {
            int combatPositionIndex = SelectedCharacter.CombatPositionIndices[i];
            if (combatPositionIndex != -1)
            {
                if (!_corps.UnJoinable(i, combatPositionIndex))
                {
                    AlertModal.Do?.Invoke("�ּ� �Ѹ��� ������ �����ϰ� �־���մϴ�.");
                }
                else
                {
                    Combatant combatant = _corps.Presets[i].Combatants[SelectedCharacter.CombatPositionIndices[i]];
                    combatant.ReleaseCharacter();
                    SelectedCharacter.Released(i);
                }
            }
        }
    }

    public void Fire()
    {
        _inventory.Remove(SelectedCharacter);
        CloseInfoUI();
    }

    public void InvestStatPoint(int index)
    {
        SelectedCharacter.InvestStatPoint((StatType)index);
    }

    public void CloseInfoUI()
    {
        CorpsUI.State = State.Idle;

        if (SelectedCharacter != null) SelectedCharacter.onValueChanged -= OnCharacterValueChanged;
        SelectedCharacter = null;

        _infoCanvas.enabled = false;
    }

    private void OnClickSlot(Character character)
    {
        switch (CorpsUI.State)
        {
            case State.Idle:
                CorpsUI.State = State.ShowCharacterInfo;
                SelectedCharacter = character;
                SelectedCharacter.onValueChanged += OnCharacterValueChanged;
                OpenInfoUI(character);
                break;

            case State.SelectCharacterSlot:
                Equip(character);
                Cutout.Cancel?.Invoke();
                break;
        }
    }

    private void OpenInfoUI(Character character)
    {
        OnCharacterValueChanged(character);
        _infoCanvas.enabled = true;
    }

    private void Equip(Character character)
    {
        Combatant combatant = CorpsUI.SelectedCombatant;
        int presetIndex = CorpsUI.SelectedPresetIndex;
        int combatPositionIndex = CorpsUI.SelectedCombatPositionIndex;

        int equippedCombatPositionIndex = character.CombatPositionIndices[presetIndex];

        if (equippedCombatPositionIndex != -1)
        {
            if (equippedCombatPositionIndex == combatPositionIndex)
                return;

            _corps.Presets[presetIndex].Combatants[combatPositionIndex].ReleaseCharacter();
        }
        if (combatant.IsCharacterEquipped)
        {
            Character replaced = combatant.ReleaseCharacter();
            replaced.Released(presetIndex);
        }
        combatant.EquipCharacter(character);
        character.Equipped(presetIndex, combatPositionIndex);
    }

    private void OnCharacterValueChanged(Character character)
    {
        // ����
        _previewImage.sprite = character.Preview;

        // �̸�
        _nameText.text = character.Name;

        // ����
        _levelText.text = $"Lv.{character.Level}";

        // ���� ����Ʈ
        _statPointText.text = $"SP {character.StatPoint}";

        // ����ġ
        int exp = character.Exp;
        int maxExp = Variables.MaxExps[character.Level];
        _expText.text = $"{exp}/{maxExp}";
        _expBarFillImage.fillAmount = exp / maxExp;

        _fireButton.interactable = !character.IsEquipped;

        // ����
        for (int i = 0; i < _statTexts.Length; i++)
            _statTexts[i].text = character.Stats[i].Value.ToString();

        // ���� ���� ��ư
        for (int i = 0; i < _investButtons.Length; i++)
            _investButtons[i].interactable = character.StatPoint > 0;
    }

    private void OnCharacterAdded(Character character)
    {
        CharacterSlot slot = Instantiate(_slotPrefab, _slotHolder);
        slot.Init(() => OnClickSlot(character));

        character.onValueChanged += slot.UpdateUI;        
    }

    private void OnCharacterRemoved(Character character)
    {
        Destroy(_slotHolder.GetChild(_inventory.Items.IndexOf(character)).gameObject);
    }
}
