using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 1f;

    [SerializeField] EnemyCheckForPlayer checkForPlayer;

    Transform player;
    float fireTime;
    Vector2 aimDirection;

    void Update()
    {
        AttackPlayer();
    }

    void AttackPlayer()
    {
        player = checkForPlayer.player;
        aimDirection = (player.position - transform.position).normalized;
        
        if (player != null)
        {
            if (fireTime < Time.time)
            {
                GameObject bulletToShoot = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bulletToShoot.GetComponent<Bullet>().FireBullet(aimDirection);
                bulletToShoot.GetComponent<Bullet>().shooterTag = gameObject.tag; // assign who fired it
                fireTime = Time.time + fireRate;
            }
        }
    }
}
