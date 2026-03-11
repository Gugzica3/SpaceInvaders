using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
        }
        // Destroy the projectile after a certain time to prevent memory leaks
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile upwards or downwards based on its rotation or speed sign (usually defined by parent or spawner)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add specific collision logic outside of Player/Enemy triggers if necessary, e.g. hitting walls
        if (collision.gameObject.name.Contains("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
