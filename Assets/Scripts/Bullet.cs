using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;

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
        shootingDirection = fireDirection;
        rb.linearVelocity = shootingDirection * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
