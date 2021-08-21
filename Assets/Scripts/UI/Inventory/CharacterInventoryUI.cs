using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterInventoryUI : BaseInventoryUI<Character, CharacterSlot>
{
    private void Start()
    {
        _channel.initCharacterInventoryUI += InitUI;
        _channel.addCharacterSlot += AddSlot;
        _channel.removeCharacterSlot += RemoveSlot;
        _channel.updateCharacterSlot += UpdateSlot;
        _channel.openCharacterInfoUIWithCutout += OpenWithCutout;
    }

    private void OnDestroy()
    {
        _channel.initCharacterInventoryUI -= InitUI;
        _channel.addCharacterSlot -= AddSlot;
        _channel.removeCharacterSlot -= RemoveSlot;
        _channel.updateCharacterSlot -= UpdateSlot;
        _channel.openCharacterInfoUIWithCutout -= OpenWithCutout;
    }

    protected override void InitUI(List<Character> characters, UnityAction<Character> selectCharacter)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            CharacterSlot slot = Instantiate(_slotPrefab, _slotHolder[0]);
            int index = i;
            slot.Init(characters[index], () => selectCharacter(characters[index]));
        }
    }

    protected override void AddSlot(Character character, UnityAction<Character> selectCharacter)
    {
        CharacterSlot slot = Instantiate(_slotPrefab, _slotHolder[0]);
        slot.Init(character, () => selectCharacter(character));
    }

    protected override void UpdateSlot(int index, Character character)
    {
        _slotHolder[0].GetChild(index).GetComponent<CharacterSlot>().UpdateUI(character);
    }
}
