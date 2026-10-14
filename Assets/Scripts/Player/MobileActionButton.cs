using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to UI button Images for Sprint, Jump, Fire, or Reload.
/// Sprint is held while the finger is down; Jump and Fire trigger once per press.
/// </summary>
public class MobileActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public enum ActionType { Sprint, Jump, Fire, Reload }

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
        else if (action == ActionType.Reload)
            MobileInputState.RequestReload();
    }

    private void OnDisable()
    {
        if (action == ActionType.Sprint)
            MobileInputState.SetSprint(false);
    }
}