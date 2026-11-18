using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 4f;

    [SerializeField] float mouseSensitivity = 0.15f;

    CharacterController controller;
    Transform camTransform;
    float pitch;
    public bool isKnockedBack;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null || mouse == null) return; // no device connected this frame

        // Esc lets go of the mouse so you can use the editor
        if (kb.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Look
        Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity;

        transform.Rotate(0f, delta.x, 0f);
        pitch = Mathf.Clamp(pitch - delta.y, -80f, 80f);
        camTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // the player cannot move around if they are knocked back
        if (isKnockedBack) { return; }
        // Move (WASD or arrow keys)
        float x = 0f, y = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) x -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) x += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) y -= 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) y += 1f;

        Vector3 move = transform.right * x + transform.forward * y;
        if (move.sqrMagnitude > 1f) move.Normalize(); // no faster diagonals

        controller.Move(move * walkSpeed * Time.deltaTime);
    }
}



