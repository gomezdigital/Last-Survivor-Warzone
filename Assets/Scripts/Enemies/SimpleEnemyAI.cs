using UnityEngine;

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
    [SerializeField, Min(0f)] private float maxHealth = 60f;
    [SerializeField, Min(0f)] private float damagePerAttack = 10f;
    [SerializeField, Min(0.1f)] private float attackInterval = 1.5f;
    [SerializeField, Min(0f)] private float attackRange = 2f;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float moveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float turnSpeed = 8f;

    private float currentHealth;
    private float nextAttackTime;
    private bool isDead;

    private void Awake()
    {
        currentHealth = Mathf.Max(1f, maxHealth);
    }

    private void Start()
    {
        if (player == null)
        {
            SurvivalStats stats = FindObjectOfType<SurvivalStats>();
            if (stats != null)
                player = stats.transform;
        }
    }

    private void Update()
    {
        if (isDead || player == null)
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
            SurvivalStats playerStats = player.GetComponent<SurvivalStats>();
            if (playerStats != null)
                playerStats.TakeDamage(damagePerAttack);

            nextAttackTime = Time.time + attackInterval;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);

        if (currentHealth <= 0f)
        {
            isDead = true;
            gameObject.SetActive(false);
        }
    }
}