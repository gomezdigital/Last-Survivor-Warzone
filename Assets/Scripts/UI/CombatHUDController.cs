using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the prototype weapon's ammunition and a simple centre-screen crosshair.
/// Assign the weapon and UI Text objects in the Inspector.
/// </summary>
public class CombatHUDController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private SimpleWeapon weapon;

    [Header("Optional UI")]
    [SerializeField] private Text ammoLabel;
    [SerializeField] private Text crosshairLabel;

    private void Awake()
    {
        if (weapon == null)
            weapon = FindObjectOfType<SimpleWeapon>();

        if (crosshairLabel != null)
            crosshairLabel.text = "+";
    }

    private void OnEnable()
    {
        if (weapon == null)
            weapon = FindObjectOfType<SimpleWeapon>();

        if (weapon != null)
        {
            weapon.AmmoChanged.AddListener(UpdateAmmo);
            UpdateAmmo(weapon.CurrentMagazineAmmo, weapon.ReserveAmmo, weapon.IsReloading);
        }
    }

    private void OnDisable()
    {
        if (weapon != null)
            weapon.AmmoChanged.RemoveListener(UpdateAmmo);
    }

    private void UpdateAmmo(int magazineAmmo, int reserveAmmo, bool isReloading)
    {
        if (ammoLabel == null)
            return;

        ammoLabel.text = isReloading
            ? $"RELOADING...  {magazineAmmo}/{reserveAmmo}"
            : $"AMMO  {magazineAmmo} | {reserveAmmo} RESERVE";
    }
}
