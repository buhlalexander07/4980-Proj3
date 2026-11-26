using System;
using UnityEngine;
using System.Collections.Generic;

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

    /// <summary>
    /// Switches to the next available equipment item.
    /// </summary>
    /// <param name="availableItems"> Equipment the player currently possesses. </param>
    public void SwitchToNextItem(List<EquipmentType> availableItems)
    {
        // Cannot switch equipment if the player has no items.
        if (availableItems.Count == 0)
        {
            return;
        }

        // Find the currently equipped item in the available items.
        int currentIndex = availableItems.IndexOf(equippedItem);

        // Move to the next item, wrapping around at the end.
        int nextIndex = (currentIndex + 1) % availableItems.Count;

        // Equip the next item using the existing equipment system.
        SetEquippedItem(availableItems[nextIndex]);
    }

    /// <summary>
    /// Switches to the previous available equipment item.
    /// </summary>
    /// <param name="availableItems"> equipment the player currently possesses. </param>
    public void SwitchToPreviousItem(List<EquipmentType> availableItems)
    {
        // Cannot switch equipment if the player has no items.
        if (availableItems.Count == 0)
        {
            return;
        }

        // Find the currently equipped item in the available items.
        int currentIndex = availableItems.IndexOf(equippedItem);

        // If the current item is unavailable, select the last item.
        if (currentIndex == -1)
        {
            SetEquippedItem(availableItems[availableItems.Count - 1]);
            return;
        }

        // Move to the previous item, wrapping around at the beginning.
        int previousIndex = (currentIndex - 1 + availableItems.Count) % availableItems.Count;

        // Equip the previous item using the existing equipment system.
        SetEquippedItem(availableItems[previousIndex]);
    }
}
