using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 7f;

    [Header("Jumping")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -9.81f;

    [Header("Looking")]
    [SerializeField] float mouseSensitivity = 0.15f;

    // the player cannot move around if they are knocked back
    public bool isKnockedBack;

    // LeadController lowers this so the player can't outrun the animal they're leading
    public float maxSpeed = Mathf.Infinity;

    CharacterController controller;
    Transform camTransform;
    float pitch;
    Vector3 velocity; // used for falling and jumping

    // Input actions from the project-wide Input Actions asset
    InputAction moveAction;
    InputAction lookAction;
    InputAction jumpAction;
    InputAction sprintAction;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Find the actions by name (these come with the default Player action map)
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void Update()
    {
        // Esc lets go of the mouse so you can use the editor
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Look();
        Move();
    }

    void Look()
    {
        Vector2 delta = lookAction.ReadValue<Vector2>() * mouseSensitivity;

        transform.Rotate(0f, delta.x, 0f);
        pitch = Mathf.Clamp(pitch - delta.y, -80f, 80f);
        camTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void Move()
    {
        bool isGrounded = controller.isGrounded;

        // Small downward push so the player stays stuck to the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!isKnockedBack)
        {
            // Walk (WASD / arrow keys / left stick)
            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 move = transform.right * input.x + transform.forward * input.y;

            // Sprint
            float speed = walkSpeed;
            if (sprintAction.IsPressed())
            {
                speed = sprintSpeed;
            }

            // Can't go faster than the animal being led
            speed = Mathf.Min(speed, maxSpeed);

            controller.Move(move * speed * Time.deltaTime);

            // Jump (only on the frame the button is pressed, and only on the ground)
            if (jumpAction.WasPressedThisFrame() && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Gravity (still applies when knocked back so the player doesn't float)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}