using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicUI : MonoBehaviour
{
    [SerializeField] private RelicInventorySO _inventory;

    [SerializeField] private RelicSlot _slotPrefab;
    [SerializeField] private Transform _slotHolder;

    private void Start()
    {
        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            RelicSlot slot = Instantiate(_slotPrefab, _slotHolder);
            RelicSO relic = _inventory.Items[i];

            relic.onValueChanged += slot.UpdateUI;
            slot.UpdateUI(relic);
        }
    }
}
