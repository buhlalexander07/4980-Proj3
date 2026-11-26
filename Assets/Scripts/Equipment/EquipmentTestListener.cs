using UnityEngine;

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
