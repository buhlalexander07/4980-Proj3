using System;
using UnityEngine;

// Identifies the different types of equipment in the game.
public enum EquipmentType
{
    None,
    Lead,
    Lantern,
    Pitchfork,
    Gun
}

// Tracks the player's currently equipped item and notifies
// other systems whenever the equipped item changes.
public class EquipmentController : MonoBehaviour
{
    [SerializeField] private EquipmentType equippedItem = EquipmentType.None;

    public EquipmentType EquippedItem => equippedItem;

    public event Action<EquipmentType> EquipmentChanged;

    /// <summary>
    /// Changes the currently equipped item and notifies other systems.
    /// </summary>
    /// <param name="item"> The new equipment type. </param>
    public void SetEquippedItem(EquipmentType item)
    {
        // No change is necessary if the item is already equipped.
        if (equippedItem == item)
        {
            return;
        }

        // Update the equipped item and notify interested systems.
        equippedItem = item;
        EquipmentChanged?.Invoke(equippedItem);
    }
}
