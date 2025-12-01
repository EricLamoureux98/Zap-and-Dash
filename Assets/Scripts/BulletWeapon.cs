using UnityEngine;

public class BulletWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] AimTowardMouse aimTowardMouse;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate;

    float fireTimer;


    public void Fire()
    {
        if (fireTimer <= 0)
        {
            GameObject bulletToShoot = Instantiate(bulletPrefab, playerShoot.currentFirePoint.position, transform.rotation);
            bulletToShoot.GetComponent<Bullet>().FireBullet(aimTowardMouse.AimDirection);
            bulletToShoot.GetComponent<Bullet>().shooterTag = gameObject.tag; // assign who fired it
            bulletToShoot.GetComponent<Bullet>().shooter = this.transform; // assign where it was shot from
            fireTimer = fireRate;
        }
    }

    public void StopFiring()
    {
        
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;
    }

}
