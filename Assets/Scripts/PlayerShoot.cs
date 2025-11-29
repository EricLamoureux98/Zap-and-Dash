using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    IWeapon currentWeapon;

    [SerializeField] BulletWeapon bulletWeapon;
    [SerializeField] LaserWeapon laserWeapon;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] AimTowardMouse aimTowardMouse;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform firePointUp;
    [SerializeField] Transform firePointDown;    

    //Vector2 shootingDirection;
    [HideInInspector] public Transform currentFirePoint;
    
    bool isFiring = false;

    void Start()
    {
        //currentWeapon = bulletWeapon;
        currentWeapon = laserWeapon;
        currentFirePoint = firePoint;
    }

    void Update()
    {      
        if (isFiring && currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    void Shoot()
    {
        // Check which way the player is facing: if facing right (localScale.x > 0), shoot right; otherwise, shoot left
        //shootingDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
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
            else
            {
                currentFirePoint = firePoint;
            }
        }
    }

    public void EquipWeapon(IWeapon weapon)
    {
        currentWeapon = weapon;
    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        FindFirePoint();
        if (context.performed && playerMovement.grounded)
        {
            isFiring = true;
        }
        
        if (context.canceled)
        {
            isFiring = false;
        }
    }
}
