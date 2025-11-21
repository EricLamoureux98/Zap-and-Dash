using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 1f;

    bool canAttack = false;
    float firerateTimer;
    Vector2 aimDirection;

    public float damage;

    void Update()
    {
        if (!canAttack) return;
    }

    public void SetActive(bool active)
    {
        canAttack = active;
    }

    public void ShootAtTarget(Transform targetPosition)
    {
        if (canAttack)
        {
            aimDirection = (targetPosition.position - transform.position).normalized;
            ShootAtPlayer(aimDirection);
        }
    }

    void ShootAtPlayer(Vector2 direction)
    {
        if (firerateTimer < Time.time)
        {
            GameObject bulletToShoot = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bulletToShoot.GetComponent<Bullet>().FireBullet(direction);
            bulletToShoot.GetComponent<Bullet>().shooterTag = gameObject.tag; // assign who fired it
            bulletToShoot.GetComponent<Bullet>().damage = damage;
            bulletToShoot.GetComponent<Bullet>().shooter = this.transform;
            firerateTimer = Time.time + fireRate;
        }
    }
}
