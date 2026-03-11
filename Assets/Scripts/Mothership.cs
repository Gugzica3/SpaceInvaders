using System.Collections;
using UnityEngine;

public class Mothership : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3.5f;

    [Header("Score")]
    public int scoreMin = 100;
    public int scoreMax = 300;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public float normalFireRate  = 4f;   // seconds between shots (normal)
    public float bossFireRate    = 1.5f; // when all enemies dead (boss mode)

    [Header("Reinforcements (if mothership escapes)")]
    public GameObject enemyPrefab;       // assign Enemy prefab in Inspector
    public int reinforcementCount = 3;

    // Internals
    private SpriteRenderer  sr;
    private float           blinkTimer  = 0f;
    private bool            isBossMode  = false;
    private Coroutine       shootRoutine;

    // Colours for blink animation
    private Color normalColor1 = Color.white;
    private Color normalColor2 = new Color(1f, 0.9f, 0.1f);   // yellow
    private Color bossColor1   = Color.white;
    private Color bossColor2   = new Color(1f, 0.1f, 0.1f);   // red

    // Track whether it was destroyed by the player or just flew off
    private bool destroyedByPlayer = false;

    // ─────────────────────────────────────────────────────────────────────────

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType     = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        shootRoutine = StartCoroutine(ShootRoutine());

        // Self-destroy after 14 s (gives it time to cross the full screen)
        Invoke(nameof(EscapeWithoutBeingHit), 14f);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // ── Boss mode check ──────────────────────────────────────────────────
        bool noEnemiesLeft = (GameManager.Instance != null) &&
                             (GameManager.Instance.enemiesDestroyed >= GameManager.Instance.totalEnemies);
        if (noEnemiesLeft && !isBossMode)
            EnterBossMode();

        // ── Blink animation ──────────────────────────────────────────────────
        float blinkRate = isBossMode ? 0.12f : 0.3f;
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkRate)
        {
            blinkTimer = 0f;
            if (sr != null)
            {
                Color c1 = isBossMode ? bossColor1 : normalColor1;
                Color c2 = isBossMode ? bossColor2 : normalColor2;
                sr.color = sr.color == c1 ? c2 : c1;
            }
        }
    }

    // ─── Boss Mode ───────────────────────────────────────────────────────────

    void EnterBossMode()
    {
        isBossMode = true;
        speed *= 1.4f;             // gets faster
        transform.localScale *= 1.3f; // gets bigger

        // Restart shoot coroutine with faster rate
        if (shootRoutine != null) StopCoroutine(shootRoutine);
        shootRoutine = StartCoroutine(ShootRoutine());

        Debug.Log("[Mothership] BOSS MODE activated!");
    }

    // ─── Shooting ────────────────────────────────────────────────────────────

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            float rate = isBossMode ? bossFireRate : normalFireRate;
            yield return new WaitForSeconds(rate);
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        if (projectilePrefab == null) return;

        // Aim downward (toward player area)
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Override to shoot straight down regardless of projectile default speed
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null) p.speed = -Mathf.Abs(p.speed);   // force downward
    }

    // ─── Collision ───────────────────────────────────────────────────────────

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            destroyedByPlayer = true;
            CancelInvoke(nameof(EscapeWithoutBeingHit));
            Destroy(collision.gameObject);
            Die();
        }
    }

    // ─── Death ───────────────────────────────────────────────────────────────

    void Die()
    {
        int pts = Random.Range(scoreMin, scoreMax);

        // Boss mode bonus: double points!
        if (isBossMode) pts *= 2;

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(pts);

        // If boss mode was active and no enemies left → trigger win
        if (isBossMode &&
            GameManager.Instance != null &&
            GameManager.Instance.enemiesDestroyed >= GameManager.Instance.totalEnemies)
        {
            GameManager.Instance.GameWon();
        }

        Destroy(gameObject);
    }

    // ─── Escape (flew off without being shot) ────────────────────────────────

    void EscapeWithoutBeingHit()
    {
        if (destroyedByPlayer) return;

        // Punishment: spawn reinforcement enemies!
        SpawnReinforcements();
        Destroy(gameObject);
    }

    void SpawnReinforcements()
    {
        if (enemyPrefab == null || GameManager.Instance == null) return;

        int count = Mathf.Min(reinforcementCount, 5);
        GameManager.Instance.totalEnemies += count; // update enemy count

        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-6f, 6f);
            float y = Random.Range(1f, 4f);
            Instantiate(enemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
        }

        Debug.Log("[Mothership] Escaped! Spawned " + count + " reinforcements.");
    }
}
