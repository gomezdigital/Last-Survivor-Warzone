using UnityEngine;

/// <summary>
/// Prototype trigger pickup that restores player health or reserve ammunition.
/// Add a trigger collider to the pickup object and assign its type/value in the Inspector.
/// </summary>
[RequireComponent(typeof(Collider))]
public class SupplyPickup : MonoBehaviour
{
    public enum SupplyType
    {
        Health,
        Ammunition
    }

    [Header("Supply")]
    [SerializeField] private SupplyType supplyType = SupplyType.Health;
    [SerializeField, Min(1f)] private float healthAmount = 25f;
    [SerializeField, Min(1)] private int ammoAmount = 18;
    [SerializeField] private bool destroyAfterCollection = true;

    private Collider pickupCollider;

    private void Awake()
    {
        pickupCollider = GetComponent<Collider>();
        pickupCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        SurvivalStats stats = other.GetComponentInParent<SurvivalStats>();
        if (stats == null || stats.IsDead)
            return;

        bool collected = false;

        if (supplyType == SupplyType.Health)
        {
            float before = stats.CurrentHealth;
            stats.Heal(healthAmount);
            collected = stats.CurrentHealth > before;
        }
        else
        {
            SimpleWeapon weapon = other.GetComponentInParent<SimpleWeapon>();
            if (weapon == null)
                weapon = stats.GetComponentInChildren<SimpleWeapon>();

            if (weapon != null)
                collected = weapon.AddReserveAmmo(ammoAmount) > 0;
        }

        if (collected && destroyAfterCollection)
            Destroy(gameObject);
    }
}
