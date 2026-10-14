using UnityEngine;

/// <summary>
/// Third-person movement controller for Editor keyboard testing and on-screen mobile controls.
/// Uses Unity's legacy Input Manager for keyboard input.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonPlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)] private float walkSpeed = 3.5f;
    [SerializeField, Min(0f)] private float sprintSpeed = 6f;
    [SerializeField, Min(0f)] private float rotationSpeed = 12f;
    [SerializeField, Min(0f)] private float jumpHeight = 1.1f;
    [SerializeField] private float gravity = -20f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private SurvivalStats survivalStats;

    private CharacterController characterController;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (survivalStats == null)
            survivalStats = GetComponent<SurvivalStats>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (gravity > -0.01f)
        {
            Debug.LogWarning("ThirdPersonPlayerController: Gravity should be negative. Resetting to -20.", this);
            gravity = -20f;
        }
    }

    private void Update()
    {
        if (characterController == null || !characterController.enabled)
            return;

        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 input2D = keyboardInput.sqrMagnitude > 0.001f ? keyboardInput : MobileInputState.Move;
        input2D = Vector2.ClampMagnitude(input2D, 1f);
        Vector3 input = new Vector3(input2D.x, 0f, input2D.y);

        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        if (cameraTransform != null)
        {
            forward = cameraTransform.forward;
            right = cameraTransform.right;
        }

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * input.z + right * input.x;
        bool sprintInput = Input.GetKey(KeyCode.LeftShift) || MobileInputState.SprintHeld;
        bool wantsToSprint = sprintInput && input.sqrMagnitude > 0.01f;
        bool canSprint = survivalStats == null || survivalStats.CurrentStamina > 0.1f;
        bool isSprinting = wantsToSprint && canSprint;
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        if (isSprinting && survivalStats != null)
            survivalStats.UseStamina(Time.deltaTime * 18f);
        else if (survivalStats != null)
            survivalStats.RestoreStamina(Time.deltaTime * 12f);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (characterController.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        bool mobileJumpRequested = MobileInputState.ConsumeJumpPressed();
        bool jumpRequested = Input.GetButtonDown("Jump") || mobileJumpRequested;
        if (characterController.isGrounded && jumpRequested)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * speed;
        velocity.y = verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }
}