using UnityEngine;

/// <summary>
/// Simple third-person follow camera for early prototypes.
/// Mouse look is intended for Editor testing; mobile drag input is not yet included.
/// </summary>
public class ThirdPersonCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Min(0.1f)] private float distance = 4.5f;
    [SerializeField, Min(0f)] private float height = 1.8f;
    [SerializeField, Min(0.1f)] private float lookSensitivity = 2f;
    [SerializeField, Range(-30f, 10f)] private float minPitch = -20f;
    [SerializeField, Range(10f, 80f)] private float maxPitch = 55f;
    [SerializeField, Min(0f)] private float positionSmooth = 12f;
    [SerializeField] private bool lockCursorInPlayMode = true;

    private float yaw;
    private float pitch = 15f;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("ThirdPersonCameraFollow: Assign a target in the Inspector.", this);
            enabled = false;
            return;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;

        if (lockCursorInPlayMode)
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        else if (lockCursorInPlayMode && Input.GetMouseButtonDown(1))
            Cursor.lockState = CursorLockMode.Locked;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * lookSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = target.position + Vector3.up * height;
        Vector3 desiredPosition = focusPoint - rotation * Vector3.forward * distance;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionSmooth * Time.deltaTime
        );

        transform.rotation = rotation;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}