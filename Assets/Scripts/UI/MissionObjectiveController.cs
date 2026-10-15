using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Tracks a small mission objective by subscribing to assigned enemies' Defeated events.
/// Assign the enemy objects in the Inspector before play. This is a prototype mission flow,
/// not a save system or a complete objective framework.
/// </summary>
public class MissionObjectiveController : MonoBehaviour
{
    [Header("Mission")]
    [SerializeField] private string missionTitle = "MISSION 001 — SECURE THE OUTPOST";
    [SerializeField, Min(1)] private int requiredKills = 3;
    [SerializeField] private SimpleEnemyAI[] objectiveEnemies;

    [Header("UI (optional)")]
    [SerializeField] private GameObject missionCompletePanel;
    [SerializeField] private Text objectiveText;
    [SerializeField] private Text completionMessage;

    [Header("Events")]
    public UnityEvent MissionCompleted = new UnityEvent();

    public int DefeatedCount { get; private set; }
    public int RequiredKills => GetRequiredKills();
    public bool IsComplete { get; private set; }

    private bool subscribed;

    private void Awake()
    {
        if (missionCompletePanel != null)
            missionCompletePanel.SetActive(false);

        if (completionMessage != null)
            completionMessage.text = missionTitle + "\nOUTPOST SECURED";
    }

    private void OnEnable()
    {
        SubscribeToEnemies();
        RefreshObjectiveText();
        CheckCompletion();
    }

    private void OnDisable()
    {
        UnsubscribeFromEnemies();
    }

    private void OnDestroy()
    {
        // A mission panel may pause the game; never leave the Editor/game paused after scene teardown.
        Time.timeScale = 1f;
    }

    private void SubscribeToEnemies()
    {
        if (subscribed || objectiveEnemies == null)
            return;

        DefeatedCount = 0;

        foreach (SimpleEnemyAI enemy in objectiveEnemies)
        {
            if (enemy == null)
                continue;

            enemy.Defeated.RemoveListener(HandleEnemyDefeated);
            enemy.Defeated.AddListener(HandleEnemyDefeated);

            // Supports a controller enabled after an enemy was already defeated.
            if (enemy.IsDead)
                DefeatedCount++;
        }

        subscribed = true;
        DefeatedCount = Mathf.Clamp(DefeatedCount, 0, GetRequiredKills());
    }

    private void UnsubscribeFromEnemies()
    {
        if (!subscribed || objectiveEnemies == null)
            return;

        foreach (SimpleEnemyAI enemy in objectiveEnemies)
        {
            if (enemy != null)
                enemy.Defeated.RemoveListener(HandleEnemyDefeated);
        }

        subscribed = false;
    }

    private void HandleEnemyDefeated()
    {
        if (IsComplete)
            return;

        DefeatedCount = Mathf.Min(DefeatedCount + 1, GetRequiredKills());
        RefreshObjectiveText();
        CheckCompletion();
    }

    private int GetRequiredKills()
    {
        if (objectiveEnemies != null && objectiveEnemies.Length > 0)
            return Mathf.Clamp(requiredKills, 1, objectiveEnemies.Length);

        return Mathf.Max(1, requiredKills);
    }

    private void RefreshObjectiveText()
    {
        if (objectiveText != null)
            objectiveText.text = missionTitle + "\nEliminate enemies: " + DefeatedCount + "/" + GetRequiredKills();
    }

    private void CheckCompletion()
    {
        if (IsComplete || DefeatedCount < GetRequiredKills())
            return;

        IsComplete = true;
        RefreshObjectiveText();

        if (missionCompletePanel != null)
            missionCompletePanel.SetActive(true);

        if (completionMessage != null)
            completionMessage.text = missionTitle + "\nOUTPOST SECURED!";

        Time.timeScale = 0f;
        MissionCompleted.Invoke();
    }

    /// <summary>Connect this method to a UI Button to reload the current mission scene.</summary>
    public void RestartMission()
    {
        Time.timeScale = 1f;
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.buildIndex >= 0)
            SceneManager.LoadScene(activeScene.buildIndex);
        else
            Debug.LogWarning("Mission restart failed: add this scene to Build Settings / Build Profiles.");
    }
}
