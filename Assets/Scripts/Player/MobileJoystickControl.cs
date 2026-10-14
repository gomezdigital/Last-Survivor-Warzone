using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Attach to a UI Image used as the left-side joystick background.
/// Assign a child handle Image. Add an EventSystem to the scene.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class MobileJoystickControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField, Min(10f)] private float movementRadius = 70f;

    private RectTransform root;

    private void Awake()
    {
        root = GetComponent<RectTransform>();
        if (handle == null)
            Debug.LogWarning("MobileJoystickControl: Assign the joystick handle.", this);
    }

    public void OnPointerDown(PointerEventData eventData) => UpdateJoystick(eventData);
    public void OnDrag(PointerEventData eventData) => UpdateJoystick(eventData);

    public void OnPointerUp(PointerEventData eventData)
    {
        MobileInputState.SetMove(Vector2.zero);
        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }

    private void UpdateJoystick(PointerEventData eventData)
    {
        if (root == null || handle == null)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                root, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            return;

        Vector2 input = Vector2.ClampMagnitude(localPoint / Mathf.Max(10f, movementRadius), 1f);
        handle.anchoredPosition = input * movementRadius;
        MobileInputState.SetMove(input);
    }

    private void OnDisable()
    {
        MobileInputState.SetMove(Vector2.zero);
    }
}