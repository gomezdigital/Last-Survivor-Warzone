using UnityEngine;

/// <summary>
/// Basic raycast weapon prototype for keyboard/mouse and mobile fire-button input.
/// Ammo, reload, fire rate, recoil, effects, and weapon animations are future tasks.
/// </summary>
public class SimpleWeapon : MonoBehaviour
{
    [SerializeField] private Camera aimCamera;
    [SerializeField, Min(0.1f)] private float range = 80f;
    [SerializeField, Min(0f)] private float damage = 25f;
    [SerializeField] private LayerMask hitLayers = Physics.DefaultRaycastLayers;

    private void Awake()
    {
        if (aimCamera == null)
            aimCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") || MobileInputState.ConsumeFirePressed())
            Fire();
    }

    public void Fire()
    {
        if (aimCamera == null)
        {
            Debug.LogWarning("SimpleWeapon: Assign an aim camera or tag the active camera MainCamera.", this);
            return;
        }

        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers, QueryTriggerInteraction.Ignore))
        {
            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damage);
        }
    }
}