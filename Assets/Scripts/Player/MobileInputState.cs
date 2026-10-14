using UnityEngine;

/// <summary>
/// Shared input values written by on-screen mobile controls and read by gameplay scripts.
/// Tap and look values are consumed once by the relevant gameplay component.
/// </summary>
public static class MobileInputState
{
    public static Vector2 Move { get; private set; }
    public static Vector2 LookDelta { get; private set; }
    public static bool SprintHeld { get; private set; }
    public static bool JumpPressed { get; private set; }
    public static bool FirePressed { get; private set; }
    public static bool ReloadPressed { get; private set; }

    public static void SetMove(Vector2 value) => Move = Vector2.ClampMagnitude(value, 1f);
    public static void SetSprint(bool held) => SprintHeld = held;
    public static void AddLookDelta(Vector2 value) => LookDelta += value;
    public static void RequestJump() => JumpPressed = true;
    public static void RequestFire() => FirePressed = true;
    public static void RequestReload() => ReloadPressed = true;

    public static bool ConsumeJumpPressed()
    {
        bool pressed = JumpPressed;
        JumpPressed = false;
        return pressed;
    }

    public static bool ConsumeFirePressed()
    {
        bool pressed = FirePressed;
        FirePressed = false;
        return pressed;
    }

    public static bool ConsumeReloadPressed()
    {
        bool pressed = ReloadPressed;
        ReloadPressed = false;
        return pressed;
    }

    public static Vector2 ConsumeLookDelta()
    {
        Vector2 delta = LookDelta;
        LookDelta = Vector2.zero;
        return delta;
    }

    public static void ResetAll()
    {
        Move = Vector2.zero;
        LookDelta = Vector2.zero;
        SprintHeld = false;
        JumpPressed = false;
        FirePressed = false;
        ReloadPressed = false;
    }
}