using UnityEngine;

/// <summary>
/// Shared input values written by on-screen mobile controls and read by gameplay scripts.
/// Reset transient inputs each frame so taps are not repeated forever.
/// </summary>
public static class MobileInputState
{
    public static Vector2 Move { get; private set; }
    public static Vector2 LookDelta { get; private set; }
    public static bool SprintHeld { get; private set; }
    public static bool JumpPressed { get; private set; }
    public static bool FirePressed { get; private set; }

    public static void SetMove(Vector2 value) => Move = Vector2.ClampMagnitude(value, 1f);
    public static void SetSprint(bool held) => SprintHeld = held;
    public static void AddLookDelta(Vector2 value) => LookDelta += value;
    public static void RequestJump() => JumpPressed = true;
    public static void RequestFire() => FirePressed = true;

    public static void EndFrame()
    {
        LookDelta = Vector2.zero;
        JumpPressed = false;
        FirePressed = false;
    }

    public static void ResetAll()
    {
        Move = Vector2.zero;
        LookDelta = Vector2.zero;
        SprintHeld = false;
        JumpPressed = false;
        FirePressed = false;
    }
}