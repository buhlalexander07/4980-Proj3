using UnityEngine;
using UnityEngine.InputSystem;

public class LeadController : MonoBehaviour
{
    [SerializeField] float reachDistance = 3f;
    [SerializeField] private EquipmentController equipmentController;

    Leadable current;   // the animal we're leading right now, null if none
    public bool aimingAtAnimal;  // true when the crosshair is over a leadable in reach
    public bool AimingAtAnimal => aimingAtAnimal;
    public bool IsLeading => current != null;

    void Update()
    {
        // Only allow leading interactions when the lead is equipped.
        if (equipmentController != null && equipmentController.EquippedItem != EquipmentType.Lead)
        {
            aimingAtAnimal = false;

            // Release the animal if the player switches away from the lead.
            if (current != null)
            {
                current.StopLeading();
                current = null;
            }

            return;
        }

        var mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        // Crosshair check
        var cam = Camera.main;
        aimingAtAnimal = false;
        if (cam != null)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hoverHit, reachDistance))
            {
                // Show an interactable crosshair only for eligible objects.
                Leadable animal = hoverHit.collider.GetComponentInParent<Leadable>();
                aimingAtAnimal = animal != null;
            }
        }

        // Grab
        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (current == null)
            {
                TryStartLead();
            }
        }

        // Release
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            if (current != null)
            {
                current.StopLeading();
                current = null;
            }
        }
    }

    /// <summary>
    /// Attempts to interact with an eligible animal in front of the player.
    /// Normal animals begin following, while monsters handle their own interaction.
    /// </summary>
    private void TryStartLead()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            return;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, reachDistance))
        {
            Leadable animal = hit.collider.GetComponentInParent<Leadable>();

            // Ignore objects that cannot be interacted with
            if (animal == null)
            {
                return;
            }

            // Let the animal decide whether to follow or trigger
            // its monster interaction.
            animal.StartLeading(transform);

            // Track an animal only if it actually started following.
            // Monsters trigger their attack instead of following.
            if (animal.IsBeingLed)
            {
                current = animal;
            }
        }
    }

    // Crosshair: a small dot in the center of the screen.
    // White normally, green when you're aiming at an animal in reach.
    void OnGUI()
    {
        var prev = GUI.color;
        GUI.color = aimingAtAnimal ? Color.green : new Color(1f, 1f, 1f, 0.7f);
        GUI.DrawTexture(new Rect((Screen.width - size) / 2f, (Screen.height - size) / 2f, size, size), Texture2D.whiteTexture);
        GUI.color = prev;
    }

    const int size = 6;
}

