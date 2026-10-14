using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to a transparent UI Image covering the right half of the screen.
/// Dragging this area rotates the camera. Keep action buttons above this area in the Canvas hierarchy.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class MobileLookAreaControl : MonoBehaviour, IDragHandler
{
    [SerializeField, Min(0.01f)] private float sensitivity = 0.12f;

    public void OnDrag(PointerEventData eventData)
    {
        MobileInputState.AddLookDelta(eventData.delta * sensitivity);
    }
}