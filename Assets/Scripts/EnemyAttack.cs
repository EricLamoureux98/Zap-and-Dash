using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float continueFiringTime;

    [SerializeField] EnemyCheckForPlayer checkForPlayer;

    Transform player;
    float firerateTimer;
    float continueFiringTimer;
    Vector2 playerLastDir;
    Vector2 aimDirection;
    bool seesPlayer;

    void Update()
    {
        seesPlayer = checkForPlayer.CheckForPlayer();

        if (seesPlayer)
        {
            Debug.Log("Player detected!");
        }
        else
        {
            Debug.Log("No player in sight.");
        }

        AttackPlayer();
        
        if (playerLastDir.magnitude > Vector2.zero.magnitude && !seesPlayer)
        {
            if (continueFiringTimer < continueFiringTime)
            {                
                ShootAtPlayer(playerLastDir);
            }
        }
    }

    void AttackPlayer()
    {
        player = checkForPlayer.player;

        if (player != null)
        {
            aimDirection = (player.position - transform.position).normalized;
            ShootAtPlayer(aimDirection);
            playerLastDir = aimDirection;
            continueFiringTimer = 0;
        }
        else if (player.position.magnitude == Vector2.zero.magnitude)
        {
            continueFiringTimer += Time.deltaTime;
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
