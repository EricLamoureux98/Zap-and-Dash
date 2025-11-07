using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;

    public string shooterTag;
    public int damage = 1;

    Rigidbody2D rb;
    Vector2 shootingDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void FireBullet(Vector2 fireDirection)
    {
        //shootingDirection = fireDirection - (Vector2)transform.position;
        shootingDirection = fireDirection;
        rb.linearVelocity = shootingDirection * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions with the shooter
        if (collision.CompareTag(shooterTag)) return;

        if (collision.CompareTag("Enemy") && shooterTag == "Player")
        {
            collision.GetComponent<Health>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player") && shooterTag == "Enemy")
        {
            collision.GetComponent<Health>()?.TakeDamage(damage);
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}
