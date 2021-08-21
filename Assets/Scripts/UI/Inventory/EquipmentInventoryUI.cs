using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentInventoryUI : BaseInventoryUI<Equipment, EquipmentSlot>
{
    [SerializeField] private RectTransform _inventoryPanel;
    [SerializeField] protected IntEventChannelSO _inventoryTabEventChannel;

    private void Start()
    {
        _channel.initEquipmentInventoryUI += InitUI;
        _channel.addEquipmentSlot += AddSlot;
        _channel.removeEquipmentSlot += RemoveSlot;
        _channel.updateEquipmentSlot += UpdateSlot;
        _channel.openEquipmentInventoryUIWithCutout += OpenWithCutout;
        _channel.openInventoryUIWithCutout += OpenTotalInventoryWithCutout;
    }

    private void OnDestroy()
    {
        _channel.initEquipmentInventoryUI -= InitUI;
        _channel.addEquipmentSlot -= AddSlot;
        _channel.removeEquipmentSlot -= RemoveSlot;
        _channel.updateEquipmentSlot -= UpdateSlot;
        _channel.openEquipmentInventoryUIWithCutout -= OpenWithCutout;
        _channel.openInventoryUIWithCutout -= OpenTotalInventoryWithCutout;
    }

    private void OpenTotalInventoryWithCutout()
    {
        _cutoutEventChannel.show?.Invoke(_panel);
        _cutoutEventChannel.onHide += () => gameObject.SetActive(false);
        _inventoryTabEventChannel.onEventRaised?.Invoke((int)InventoryTab.Equipment);

        gameObject.SetActive(true);
    }

    protected override void InitUI(List<Equipment> equipments, UnityAction<Equipment> selectEquipment)
    {
        for (int i = 0; i < equipments.Count; i++)
        {
            EquipmentSlot slot = Instantiate(_slotPrefab, _slotHolder[(int)equipments[i].Info.Type]);
            int index = i;
            slot.Init(equipments[index], () => selectEquipment(equipments[index]));
        }
    }

    protected override void AddSlot(Equipment equipment, UnityAction<Equipment> selectEquipment)
    {
        EquipmentSlot slot = Instantiate(_slotPrefab, _slotHolder[(int)equipment.Info.Type]);
        slot.Init(equipment, () => selectEquipment(equipment));
    }

    protected override void UpdateSlot(int index, Equipment equipment)
    {
        _slotHolder[(int)equipment.Info.Type].GetChild(index).GetComponent<EquipmentSlot>().UpdateUI(equipment);
    }
}
