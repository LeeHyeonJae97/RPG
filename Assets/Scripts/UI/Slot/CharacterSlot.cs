using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] protected GameObject _equippedMarkingImage;

    public void Init(UnityAction onClick)
    {
        GetComponent<Button>().onClick.AddListener(onClick);
    }

    public void UpdateUI(Character character)
    {
        _previewImage.sprite = character.Preview;
        _nameText.text = character.Name;
        _levelText.text = $"Lv.{character.Level}";
        _equippedMarkingImage.SetActive(character.IsEquipped);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
