using UnityEngine;
using UnityEngine.InputSystem;

// Temporarily changes equipment with number keys for testing.
public class EquipmentLeadTestInput : MonoBehaviour
{
    private EquipmentController equipmentController;

    void Start()
    {
        equipmentController = GetComponent<EquipmentController>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        // Press 1 to equip the lead.
        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            equipmentController.SetEquippedItem(EquipmentType.Lead);
        }

        // Press 2 to equip the lantern.
        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            equipmentController.SetEquippedItem(EquipmentType.Lantern);
        }
    }
}
