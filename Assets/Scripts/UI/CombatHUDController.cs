using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays ammunition, a centre-screen crosshair, and a brief hit marker.
/// Assign the weapon and UI Text objects in the Inspector.
/// </summary>
public class CombatHUDController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private SimpleWeapon weapon;

    [Header("Optional UI")]
    [SerializeField] private Text ammoLabel;
    [SerializeField] private Text crosshairLabel;

    [Header("Hit feedback")]
    [SerializeField, Min(0.05f)] private float hitMarkerDuration = 0.14f;

    private Color defaultCrosshairColor = Color.white;
    private Coroutine hitMarkerRoutine;

    private void Awake()
    {
        if (weapon == null)
            weapon = FindObjectOfType<SimpleWeapon>();

        if (crosshairLabel != null)
        {
            crosshairLabel.text = "+";
            defaultCrosshairColor = crosshairLabel.color;
        }
    }

    private void OnEnable()
    {
        if (weapon == null)
            weapon = FindObjectOfType<SimpleWeapon>();

        if (weapon != null)
        {
            weapon.AmmoChanged.AddListener(UpdateAmmo);
            weapon.HitConfirmed.AddListener(ShowHitMarker);
            UpdateAmmo(weapon.CurrentMagazineAmmo, weapon.ReserveAmmo, weapon.IsReloading);
        }
    }

    private void OnDisable()
    {
        if (weapon != null)
        {
            weapon.AmmoChanged.RemoveListener(UpdateAmmo);
            weapon.HitConfirmed.RemoveListener(ShowHitMarker);
        }

        if (hitMarkerRoutine != null)
        {
            StopCoroutine(hitMarkerRoutine);
            hitMarkerRoutine = null;
        }

        ResetCrosshair();
    }

    private void UpdateAmmo(int magazineAmmo, int reserveAmmo, bool isReloading)
    {
        if (ammoLabel == null)
            return;

        ammoLabel.text = isReloading
            ? $"RELOADING...  {magazineAmmo}/{reserveAmmo}"
            : $"AMMO  {magazineAmmo} | {reserveAmmo} RESERVE";
    }

    private void ShowHitMarker()
    {
        if (crosshairLabel == null)
            return;

        if (hitMarkerRoutine != null)
            StopCoroutine(hitMarkerRoutine);

        hitMarkerRoutine = StartCoroutine(HitMarkerRoutine());
    }

    private IEnumerator HitMarkerRoutine()
    {
        crosshairLabel.text = "X";
        crosshairLabel.color = Color.red;

        yield return new WaitForSeconds(hitMarkerDuration);

        ResetCrosshair();
        hitMarkerRoutine = null;
    }

    private void ResetCrosshair()
    {
        if (crosshairLabel == null)
            return;

        crosshairLabel.text = "+";
        crosshairLabel.color = defaultCrosshairColor;
    }
}
