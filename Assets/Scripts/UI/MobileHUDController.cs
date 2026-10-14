using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates simple on-screen health and stamina bars plus optional text labels.
/// Create Slider UI objects and assign them in the Inspector.
/// </summary>
public class MobileHUDController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private SurvivalStats playerStats;

    [Header("Bars")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;

    [Header("Optional labels")]
    [SerializeField] private Text healthLabel;
    [SerializeField] private Text staminaLabel;

    private void Awake()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<SurvivalStats>();

        ConfigureBar(healthBar);
        ConfigureBar(staminaBar);
    }

    private void OnEnable()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<SurvivalStats>();

        if (playerStats != null)
        {
            playerStats.HealthChanged.AddListener(UpdateHealth);
            playerStats.StaminaChanged.AddListener(UpdateStamina);
            UpdateHealth(playerStats.CurrentHealth, playerStats.MaxHealth);
            UpdateStamina(playerStats.CurrentStamina, playerStats.MaxStamina);
        }
    }

    private void OnDisable()
    {
        if (playerStats != null)
        {
            playerStats.HealthChanged.RemoveListener(UpdateHealth);
            playerStats.StaminaChanged.RemoveListener(UpdateStamina);
        }
    }

    private static void ConfigureBar(Slider bar)
    {
        if (bar == null)
            return;

        bar.minValue = 0f;
        bar.maxValue = 1f;
        bar.wholeNumbers = false;
    }

    private void UpdateHealth(float current, float maximum)
    {
        if (healthBar != null)
            healthBar.value = maximum > 0f ? Mathf.Clamp01(current / maximum) : 0f;

        if (healthLabel != null)
            healthLabel.text = $"HEALTH  {Mathf.CeilToInt(current)}/{Mathf.CeilToInt(maximum)}";
    }

    private void UpdateStamina(float current, float maximum)
    {
        if (staminaBar != null)
            staminaBar.value = maximum > 0f ? Mathf.Clamp01(current / maximum) : 0f;

        if (staminaLabel != null)
            staminaLabel.text = $"STAMINA  {Mathf.CeilToInt(current)}/{Mathf.CeilToInt(maximum)}";
    }
}