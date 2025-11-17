using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] AimTowardMouse aimTowardMouse;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform firePointUp;
    [SerializeField] Transform firePointDown;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float attackSpeed;

    Vector2 shootingDirection;
    float nextFireTime;
    Transform currentFirePoint;

    void Shoot()
    {
        FindFirePoint();
        // Check which way the player is facing: if facing right (localScale.x > 0), shoot right; otherwise, shoot left
        shootingDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        GameObject bulletToShoot = Instantiate(bulletPrefab, currentFirePoint.position, transform.rotation);
        bulletToShoot.GetComponent<Bullet>().FireBullet(aimTowardMouse.AimDirection);
        bulletToShoot.GetComponent<Bullet>().shooterTag = gameObject.tag; // assign who fired it
        bulletToShoot.GetComponent<Bullet>().shooter = this.transform; // assign where it was shot from
    }

    void FindFirePoint()
    {
        if (playerMovement.isMoving)
        {
            if (aimTowardMouse.AimDirection.y > 0.5f)
            {
                currentFirePoint = firePointUp;
            }
            else if (aimTowardMouse.AimDirection.y < -0.5f)
            {
                currentFirePoint = firePointDown;
            }
        }
        currentFirePoint = firePoint;
    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && playerMovement.grounded)
        {
            Shoot();
        }
    }
}
