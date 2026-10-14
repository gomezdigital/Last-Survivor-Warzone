using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to UI button Images for Sprint, Jump, or Fire.
/// Sprint is held while the finger is down; Jump and Fire trigger once per press.
/// </summary>
public class MobileActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public enum ActionType { Sprint, Jump, Fire }

    [SerializeField] private ActionType action = ActionType.Fire;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (action == ActionType.Sprint)
            MobileInputState.SetSprint(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (action == ActionType.Sprint)
            MobileInputState.SetSprint(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (action == ActionType.Jump)
            MobileInputState.RequestJump();
        else if (action == ActionType.Fire)
            MobileInputState.RequestFire();
    }

    private void OnDisable()
    {
        if (action == ActionType.Sprint)
            MobileInputState.SetSprint(false);
    }
}