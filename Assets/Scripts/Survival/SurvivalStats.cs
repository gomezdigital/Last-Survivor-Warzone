using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Basic health and stamina values for the first playable prototype.
/// UI, persistence, healing items, and death/restart flow are separate tasks.
/// </summary>
public class SurvivalStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField, Min(1f)] private float maxHealth = 100f;
    [SerializeField, Min(0f)] private float startingHealth = 100f;

    [Header("Stamina")]
    [SerializeField, Min(1f)] private float maxStamina = 100f;
    [SerializeField, Min(0f)] private float startingStamina = 100f;

    public UnityEvent<float, float> HealthChanged;
    public UnityEvent<float, float> StaminaChanged;
    public UnityEvent Died;

    public float CurrentHealth { get; private set; }
    public float CurrentStamina { get; private set; }
    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        maxStamina = Mathf.Max(1f, maxStamina);
        CurrentHealth = Mathf.Clamp(startingHealth, 0f, maxHealth);
        CurrentStamina = Mathf.Clamp(startingStamina, 0f, maxStamina);
        IsDead = CurrentHealth <= 0f;
    }

    private void Start()
    {
        HealthChanged?.Invoke(CurrentHealth, maxHealth);
        StaminaChanged?.Invoke(CurrentStamina, maxStamina);

        if (IsDead)
            Died?.Invoke();
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, maxHealth);
        HealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f)
        {
            IsDead = true;
            Died?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f)
            return;

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, maxHealth);
        HealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void UseStamina(float amount)
    {
        if (amount <= 0f)
            return;

        CurrentStamina = Mathf.Clamp(CurrentStamina - amount, 0f, maxStamina);
        StaminaChanged?.Invoke(CurrentStamina, maxStamina);
    }

    public void RestoreStamina(float amount)
    {
        if (amount <= 0f)
            return;

        CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0f, maxStamina);
        StaminaChanged?.Invoke(CurrentStamina, maxStamina);
    }
}