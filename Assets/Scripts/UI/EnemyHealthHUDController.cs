using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows the selected enemy's health when it is within range of the player.
/// Assign a Slider and optional Text label in the Inspector.
/// </summary>
public class EnemyHealthHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private SimpleEnemyAI targetEnemy;

    [Header("Display")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthLabel;
    [SerializeField, Min(1f)] private float visibleRange = 25f;

    private void Awake()
    {
        ConfigureBar();
        if (player == null)
        {
            SurvivalStats stats = FindObjectOfType<SurvivalStats>();
            if (stats != null)
                player = stats.transform;
        }
    }

    private void OnEnable()
    {
        SubscribeToTarget();
        RefreshDisplay();
    }

    private void OnDisable()
    {
        UnsubscribeFromTarget();
    }

    private void Update()
    {
        if (targetEnemy == null || targetEnemy.IsDead)
            FindNearestEnemy();

        RefreshDisplay();
    }

    private void FindNearestEnemy()
    {
        SimpleEnemyAI[] enemies = FindObjectsOfType<SimpleEnemyAI>();
        SimpleEnemyAI nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (SimpleEnemyAI enemy in enemies)
        {
            if (enemy == null || enemy.IsDead)
                continue;

            if (player == null)
            {
                nearest = enemy;
                break;
            }

            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance <= visibleRange && distance < nearestDistance)
            {
                nearest = enemy;
                nearestDistance = distance;
            }
        }

        if (nearest == targetEnemy)
            return;

        UnsubscribeFromTarget();
        targetEnemy = nearest;
        SubscribeToTarget();
    }

    private void SubscribeToTarget()
    {
        if (targetEnemy != null)
            targetEnemy.HealthChanged.AddListener(UpdateHealth);
    }

    private void UnsubscribeFromTarget()
    {
        if (targetEnemy != null)
            targetEnemy.HealthChanged.RemoveListener(UpdateHealth);
    }

    private void UpdateHealth(float current, float maximum)
    {
        if (healthBar != null)
            healthBar.value = maximum > 0f ? Mathf.Clamp01(current / maximum) : 0f;

        if (healthLabel != null)
            healthLabel.text = current > 0f
                ? $"ENEMY  {Mathf.CeilToInt(current)}/{Mathf.CeilToInt(maximum)}"
                : "ENEMY ELIMINATED";
    }

    private void RefreshDisplay()
    {
        bool visible = targetEnemy != null && !targetEnemy.IsDead;

        if (visible && player != null)
            visible = Vector3.Distance(player.position, targetEnemy.transform.position) <= visibleRange;

        if (healthBar != null)
            healthBar.gameObject.SetActive(visible);

        if (healthLabel != null)
            healthLabel.gameObject.SetActive(visible);

        if (visible)
            UpdateHealth(targetEnemy.CurrentHealth, targetEnemy.MaxHealth);
    }

    private void ConfigureBar()
    {
        if (healthBar == null)
            return;

        healthBar.minValue = 0f;
        healthBar.maxValue = 1f;
        healthBar.wholeNumbers = false;
    }
}
