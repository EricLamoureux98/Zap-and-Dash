using Unity.Mathematics;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] float followSpeed;
    [SerializeField] float viewDistance;
    [SerializeField] float shootingRange;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 1f;
    //[SerializeField] Transform visionPoint;

    private Transform player;
    private float fireTime;

    void Start()
    {
        // IMPROVE THIS
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < viewDistance && distanceFromPlayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= shootingRange && fireTime < Time.time)
        {
            GameObject bulletToShoot = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            bulletToShoot.GetComponent<Bullet>().FireBullet(player.position);
            bulletToShoot.GetComponent<Bullet>().shooterTag = gameObject.tag; // assign who fired it
            fireTime = Time.time + fireRate;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
