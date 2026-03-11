using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public int  currentScore    = 0;
    public int  totalEnemies    = 0;
    public int  enemiesDestroyed = 0;
    public float speedMultiplier    = 1f;
    public float speedIncreaseFactor = 1.05f; // 5% faster per kill

    [Header("Mothership Spawn")]
    public GameObject mothershipPrefab;
    public Transform  mothershipSpawnPointLeft;
    public Transform  mothershipSpawnPointRight;
    public float      mothershipSpawnMinTime = 15f;
    public float      mothershipSpawnMaxTime = 30f;

    [Header("Scene Names")]
    public string victorySceneName = "VictoryScene";
    public string defeatSceneName  = "DefeatScene";

    private bool isGameOver = false;

    void Awake()
    {
        Time.timeScale = 1f;

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        totalEnemies = enemies.Length;

        StartCoroutine(SpawnMothershipRoutine());
    }

    // ───────── Score / Lives ─────────

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        currentScore += amount;
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateScoreUI(currentScore);
    }

    public void UpdateLives(int lives)
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateLivesUI(lives);
    }

    // ───────── Enemy destroyed ─────────

    public void EnemyDestroyed()
    {
        if (isGameOver) return;

        enemiesDestroyed++;
        speedMultiplier *= speedIncreaseFactor;

        // Speed up remaining enemies
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
        {
            if (e != null)
                e.speed = Mathf.Abs(e.speed) * speedMultiplier / (speedMultiplier / speedIncreaseFactor);
        }

        if (enemiesDestroyed >= totalEnemies)
            GameWon();
    }

    // ───────── Mothership ─────────

    IEnumerator SpawnMothershipRoutine()
    {
        while (!isGameOver)
        {
            float wait = Random.Range(mothershipSpawnMinTime, mothershipSpawnMaxTime);
            yield return new WaitForSeconds(wait);
            SpawnMothership();
        }
    }

    void SpawnMothership()
    {
        if (mothershipPrefab == null || isGameOver) return;

        bool spawnLeft = Random.value > 0.5f;
        Transform spawnPoint = spawnLeft ? mothershipSpawnPointLeft : mothershipSpawnPointRight;
        if (spawnPoint == null) return;

        GameObject ms = Instantiate(mothershipPrefab, spawnPoint.position, Quaternion.identity);
        Mothership msComp = ms.GetComponent<Mothership>();
        if (msComp != null && !spawnLeft)
            msComp.speed *= -1f; // fly left
    }

    // ───────── Win / Lose ─────────

    public void GameWon()
    {
        if (isGameOver) return;
        isGameOver = true;
        PlayerPrefs.SetInt("FinalScore", currentScore);
        PlayerPrefs.Save();

        StartCoroutine(LoadSceneDelayed(victorySceneName, 1f));
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        PlayerPrefs.SetInt("FinalScore", currentScore);
        PlayerPrefs.Save();

        StartCoroutine(LoadSceneDelayed(defeatSceneName, 1f));
    }

    IEnumerator LoadSceneDelayed(string sceneName, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SpaceInvadersScene");
    }
}
