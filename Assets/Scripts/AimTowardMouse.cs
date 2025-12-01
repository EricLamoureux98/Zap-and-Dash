using UnityEngine;
using UnityEngine.InputSystem;

public class AimTowardMouse : MonoBehaviour
{
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] LaserWeapon laserWeapon;
    public Vector2 AimDirection { get; private set; }

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AimAtMouse();
    }
    
    void AimAtMouse()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        AimDirection = (mouseWorldPos - (Vector2)transform.position).normalized;

        if ((object)playerShoot.currentWeapon != laserWeapon)
        {
            anim.SetFloat("aimVertical", AimDirection.y);               
        }
    }
}
