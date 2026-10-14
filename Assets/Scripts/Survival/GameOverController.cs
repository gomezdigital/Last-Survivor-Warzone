using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Shows a defeat panel when the player's health reaches zero.
/// Connect a visible Restart button to RestartMission in the Inspector.
/// </summary>
public class GameOverController : MonoBehaviour
{
    [SerializeField] private SurvivalStats playerStats;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Text defeatMessage;

    private void Awake()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<SurvivalStats>();

        if (defeatPanel != null)
            defeatPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<SurvivalStats>();

        if (playerStats != null)
            playerStats.Died.AddListener(ShowDefeat);
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (playerStats != null && playerStats.IsDead)
            ShowDefeat();
    }

    private void OnDisable()
    {
        if (playerStats != null)
            playerStats.Died.RemoveListener(ShowDefeat);
    }

    private void ShowDefeat()
    {
        if (defeatPanel != null)
            defeatPanel.SetActive(true);
        else
            Debug.LogWarning("GameOverController: Assign a defeat panel in the Inspector.", this);

        if (defeatMessage != null)
            defeatMessage.text = "MISSION FAILED\nYou were eliminated.";

        Time.timeScale = 0f;
        MobileInputState.ResetAll();

        // Freeze gameplay scripts explicitly as well as pausing time.
        ThirdPersonPlayerController movement = FindObjectOfType<ThirdPersonPlayerController>();
        if (movement != null)
            movement.enabled = false;

        SimpleWeapon[] weapons = FindObjectsOfType<SimpleWeapon>();
        foreach (SimpleWeapon weapon in weapons)
            weapon.enabled = false;

        SimpleEnemyAI[] enemies = FindObjectsOfType<SimpleEnemyAI>();
        foreach (SimpleEnemyAI enemy in enemies)
            enemy.enabled = false;
    }

    public void RestartMission()
    {
        Time.timeScale = 1f;

        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex < 0)
        {
            Debug.LogWarning(
                "RestartMission: Add the current scene to Build Settings before testing restart.",
                this
            );
            return;
        }

        SceneManager.LoadScene(activeScene.buildIndex);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}