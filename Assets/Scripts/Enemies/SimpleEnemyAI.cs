using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple distance-based enemy prototype for a flat test arena.
/// This does not avoid obstacles; replace with NavMesh-based navigation for real levels.
/// </summary>
[RequireComponent(typeof(Collider))]
public class SimpleEnemyAI : MonoBehaviour, IDamageable
{
    [Header("Target")]
    [SerializeField] private Transform player;
    [SerializeField, Min(0.1f)] private float detectionRange = 18f;
    [SerializeField, Min(0f)] private float stopDistance = 1.8f;

    [Header("Combat")]
    [SerializeField, Min(1f)] private float maxHealth = 60f;
    [SerializeField, Min(0f)] private float damagePerAttack = 10f;
    [SerializeField, Min(0.1f)] private float attackInterval = 1.5f;
    [SerializeField, Min(0f)] private float attackRange = 2f;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float moveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float turnSpeed = 8f;

    public UnityEvent<float, float> HealthChanged = new UnityEvent<float, float>();
    public UnityEvent Defeated = new UnityEvent();

    public float CurrentHealth { get; private set; }
    public float MaxHealth => Mathf.Max(1f, maxHealth);
    public bool IsDead { get; private set; }

    private float nextAttackTime;
    private SurvivalStats cachedPlayerStats;

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        ResolvePlayer();
        HealthChanged.Invoke(CurrentHealth, MaxHealth);
    }

    private void Update()
    {
        if (IsDead || player == null)
            return;

        if (cachedPlayerStats == null)
            cachedPlayerStats = player.GetComponent<SurvivalStats>();

        if (cachedPlayerStats != null && cachedPlayerStats.IsDead)
            return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;

        if (distance > detectionRange)
            return;

        if (toPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                turnSpeed * Time.deltaTime
            );
        }

        if (distance > stopDistance)
        {
            transform.position += toPlayer.normalized * moveSpeed * Time.deltaTime;
            return;
        }

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            if (cachedPlayerStats != null)
                cachedPlayerStats.TakeDamage(damagePerAttack);

            nextAttackTime = Time.time + attackInterval;
        }
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, MaxHealth);
        HealthChanged.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            IsDead = true;
            Defeated.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void ResolvePlayer()
    {
        if (player == null)
        {
            SurvivalStats stats = FindObjectOfType<SurvivalStats>();
            if (stats != null)
                player = stats.transform;
        }

        cachedPlayerStats = player != null ? player.GetComponent<SurvivalStats>() : null;
    }
}
