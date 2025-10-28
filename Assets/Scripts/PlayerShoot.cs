using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float attackSpeed;

    public Vector2 shootingDirection { get; private set; }

    float nextFireTime;

    void Shoot()
    {
        // Check which way the player is facing: if facing right (localScale.x > 0), shoot right; otherwise, shoot left
        shootingDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        GameObject bulletToShoot = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        bulletToShoot.GetComponent<Bullet>().FireBullet(shootingDirection);
    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && playerMovement.grounded)
        {
            Shoot();
        }
    }
}
