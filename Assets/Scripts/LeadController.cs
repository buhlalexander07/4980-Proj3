using UnityEngine;
using UnityEngine.InputSystem;

public class LeadController : MonoBehaviour
{
    [SerializeField] float reachDistance = 3f;

    Leadable current;   // the animal we're leading right now, null if none
    bool aimingAtAnimal;  // true when the crosshair is over a leadable in reach

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        // Crosshair check
        var cam = Camera.main;
        aimingAtAnimal = false;
        if (cam != null)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hoverHit, reachDistance))
                aimingAtAnimal = hoverHit.collider.GetComponent<Leadable>() != null;
        }

        // Grab
        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (current == null)
                TryStartLead();
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

    void TryStartLead()
    {
        var cam = Camera.main;
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, reachDistance))
        {
            Leadable animal = hit.collider.GetComponent<Leadable>();
            if (animal != null)
            {
                animal.StartLeading(transform);
                if (!animal.isMonster)
                    current = animal;   // monsters can't be led, only attack
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

