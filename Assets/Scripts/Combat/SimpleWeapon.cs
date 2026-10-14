using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Prototype raycast weapon with magazine ammo, reload timing, and a fire-rate limit.
/// Connect an on-screen MobileActionButton set to Reload for touch input.
/// </summary>
public class SimpleWeapon : MonoBehaviour
{
    [Header("Aim and damage")]
    [SerializeField] private Camera aimCamera;
    [SerializeField, Min(0.1f)] private float range = 80f;
    [SerializeField, Min(0f)] private float damage = 25f;
    [SerializeField] private LayerMask hitLayers = Physics.DefaultRaycastLayers;

    [Header("Ammunition")]
    [SerializeField, Min(1)] private int magazineSize = 12;
    [SerializeField, Min(0f)] private int startingReserveAmmo = 60;
    [SerializeField, Min(0.05f)] private float secondsBetweenShots = 0.25f;
    [SerializeField, Min(0.1f)] private float reloadDuration = 1.6f;

    [Header("Player")]
    [SerializeField] private SurvivalStats playerStats;

    public UnityEvent<int, int, bool> AmmoChanged = new UnityEvent<int, int, bool>();

    public int CurrentMagazineAmmo { get; private set; }
    public int ReserveAmmo { get; private set; }
    public int MagazineSize => magazineSize;
    public bool IsReloading { get; private set; }

    private float nextAllowedShotTime;
    private Coroutine reloadRoutine;

    private void Awake()
    {
        magazineSize = Mathf.Max(1, magazineSize);
        startingReserveAmmo = Mathf.Max(0, startingReserveAmmo);
        secondsBetweenShots = Mathf.Max(0.05f, secondsBetweenShots);
        reloadDuration = Mathf.Max(0.1f, reloadDuration);

        CurrentMagazineAmmo = magazineSize;
        ReserveAmmo = startingReserveAmmo;

        if (aimCamera == null)
            aimCamera = Camera.main;

        if (playerStats == null)
            playerStats = GetComponentInParent<SurvivalStats>();

        if (playerStats == null)
            playerStats = FindObjectOfType<SurvivalStats>();
    }

    private void Start()
    {
        NotifyAmmoChanged();
    }

    private void Update()
    {
        bool reloadRequested = Input.GetKeyDown(KeyCode.R) || MobileInputState.ConsumeReloadPressed();
        if (reloadRequested)
            Reload();

        bool fireRequested = Input.GetButtonDown("Fire1") || MobileInputState.ConsumeFirePressed();
        if (fireRequested)
            Fire();
    }

    public void Fire()
    {
        if (IsReloading || Time.time < nextAllowedShotTime || CurrentMagazineAmmo <= 0)
            return;

        if (playerStats != null && playerStats.IsDead)
            return;

        CurrentMagazineAmmo--;
        nextAllowedShotTime = Time.time + secondsBetweenShots;
        NotifyAmmoChanged();

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

    public void Reload()
    {
        if (IsReloading || CurrentMagazineAmmo >= magazineSize || ReserveAmmo <= 0)
            return;

        if (playerStats != null && playerStats.IsDead)
            return;

        if (reloadRoutine != null)
            StopCoroutine(reloadRoutine);

        reloadRoutine = StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        IsReloading = true;
        NotifyAmmoChanged();

        yield return new WaitForSeconds(reloadDuration);

        int needed = magazineSize - CurrentMagazineAmmo;
        int loaded = Mathf.Min(needed, ReserveAmmo);
        CurrentMagazineAmmo += loaded;
        ReserveAmmo -= loaded;

        IsReloading = false;
        reloadRoutine = null;
        NotifyAmmoChanged();
    }

    private void NotifyAmmoChanged()
    {
        AmmoChanged.Invoke(CurrentMagazineAmmo, ReserveAmmo, IsReloading);
    }

    private void OnDisable()
    {
        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
            reloadRoutine = null;
        }

        IsReloading = false;
    }
}