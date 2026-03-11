using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Movement
    public float speed = 2.0f;
    public float stepDown = 0.5f;         // How far enemies drop each time they hit a wall
    private float moveDir = 1f;           // 1 = right, -1 = left
    private float edgeLeft  = -7.5f;
    private float edgeRight =  7.5f;

    // Shooting
    public int scoreValue = 10;
    public GameObject projectilePrefab;
    public float minFireRate = 2f;
    public float maxFireRate = 6f;

    // Animation (two alternating sprites)
    public Sprite frame1;
    public Sprite frame2;
    public float animFps = 1.5f;          // Times per second to flip frame
    private SpriteRenderer sr;
    private float animTimer = 0f;
    private bool showFrame1 = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.bodyType     = RigidbodyType2D.Kinematic;
            rb2d.gravityScale = 0f;
            rb2d.freezeRotation = true;
        }

        StartCoroutine(ShootRoutine());
    }

    void Update()
    {
        // --- Movement ---
        transform.Translate(Vector2.right * moveDir * speed * Time.deltaTime);

        float posX = transform.position.x;
        if (moveDir > 0 && posX >= edgeRight)
        {
            ReverseAndStepDown();
        }
        else if (moveDir < 0 && posX <= edgeLeft)
        {
            ReverseAndStepDown();
        }

        // --- Animation ---
        animTimer += Time.deltaTime;
        if (animTimer >= 1f / animFps)
        {
            animTimer = 0f;
            showFrame1 = !showFrame1;
            if (sr != null)
            {
                Sprite target = showFrame1 ? frame1 : frame2;
                if (target != null) sr.sprite = target;
            }
        }

        // --- Invasion check: enemy reached the player's row ---
        if (transform.position.y <= -4.0f && GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }

    void ReverseAndStepDown()
    {
        moveDir *= -1f;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y - stepDown,
            transform.position.z
        );
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            float wait = Random.Range(minFireRate, maxFireRate);
            yield return new WaitForSeconds(wait);
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null)
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            Destroy(collision.gameObject);
            Die();
        }
    }

    public void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.EnemyDestroyed();
        }
        Destroy(gameObject);
    }
}
