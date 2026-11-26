using UnityEngine;
using System.Collections.Generic;

// Tests equipment changes and their notifications.
public class EquipmentTestListener : MonoBehaviour
{
    [SerializeField] private EquipmentController equipmentController;

    private void OnEnable()
    {
        equipmentController.EquipmentChanged += OnEquipmentChanged;
    }

    private void Start()
    {
        // Equip the lead.
        equipmentController.SetEquippedItem(EquipmentType.Lead);

        // Try equipping the same item again.
        equipmentController.SetEquippedItem(EquipmentType.Lead);

        // Switch to the lantern.
        equipmentController.SetEquippedItem(EquipmentType.Lantern);

        Debug.Log("Final equipment: " + equipmentController.EquippedItem);

        // Test switching between available equipment items.
        List<EquipmentType> availableItems = new List<EquipmentType>
        {
            EquipmentType.Lead,
            EquipmentType.Lantern,
            EquipmentType.Pitchfork
        };

        equipmentController.SwitchToNextItem(availableItems);
        equipmentController.SwitchToNextItem(availableItems);
        equipmentController.SwitchToNextItem(availableItems);

        Debug.Log("Final item after switching: " + equipmentController.EquippedItem);

        // Test switching backward through available equipment.
        equipmentController.SwitchToPreviousItem(availableItems);
        equipmentController.SwitchToPreviousItem(availableItems);
        equipmentController.SwitchToPreviousItem(availableItems);

        Debug.Log("Final item after reverse switching: " + equipmentController.EquippedItem);

        // Test switching with no available equipment
        List<EquipmentType> emptyItems = new List<EquipmentType>();

        equipmentController.SwitchToNextItem(emptyItems);
        equipmentController.SwitchToPreviousItem(emptyItems);

        Debug.Log("After empty list: " + equipmentController.EquippedItem);

        // Test switching when nothing is equipped.
        equipmentController.SetEquippedItem(EquipmentType.None);
        equipmentController.SwitchToNextItem(availableItems);

        Debug.Log("Forward from None: " + equipmentController.EquippedItem);

        equipmentController.SetEquippedItem(EquipmentType.None);
        equipmentController.SwitchToPreviousItem(availableItems);

        Debug.Log("Backward from None: " + equipmentController.EquippedItem);

        // Test switching when only one item is available.
        List<EquipmentType> oneItem = new List<EquipmentType>
        {
            EquipmentType.Gun
        };

        equipmentController.SetEquippedItem(EquipmentType.Gun);
        equipmentController.SwitchToNextItem(oneItem);
        equipmentController.SwitchToPreviousItem(oneItem);

        Debug.Log("After single-item switching: " + equipmentController.EquippedItem);
    }

    private void OnDisable()
    {
        equipmentController.EquipmentChanged -= OnEquipmentChanged;
    }

    private void OnEquipmentChanged(EquipmentType item)
    {
        Debug.Log("Equipment changed to: " + item);
    }
}
