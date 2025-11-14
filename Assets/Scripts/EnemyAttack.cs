using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float continueFiringTime;

    [SerializeField] EnemyCheckForPlayer checkForPlayer;

    bool canAttack = false;
    Transform player;
    float firerateTimer;
    float continueFiringTimer;
    Vector2 playerLastDir;
    Vector2 aimDirection;
    bool seesPlayer;

    void Update()
    {
        if (!canAttack) return;

        seesPlayer = checkForPlayer.CheckForPlayer();

        AttackPlayer();

        if (playerLastDir.magnitude > Vector2.zero.magnitude && !seesPlayer)
        {
            if (continueFiringTimer < continueFiringTime)
            {
                continueFiringTimer += Time.deltaTime;
                ShootAtPlayer(playerLastDir);
            }
        }
        else
        {
            continueFiringTimer = 0;
        }
    }

    public void SetActive(bool active)
    {
        canAttack = active;
    }

    void AttackPlayer()
    {
        player = checkForPlayer.player;

        if (player != null)
        {
            aimDirection = (player.position - transform.position).normalized;
            ShootAtPlayer(aimDirection);
            playerLastDir = aimDirection;            
        }
    }

    void ShootAtPlayer(Vector2 direction)
    {
        if (firerateTimer < Time.time)
        {
            GameObject bulletToShoot = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bulletToShoot.GetComponent<Bullet>().FireBullet(direction);
            bulletToShoot.GetComponent<Bullet>().shooterTag = gameObject.tag; // assign who fired it
            firerateTimer = Time.time + fireRate;
        }
    }
}
