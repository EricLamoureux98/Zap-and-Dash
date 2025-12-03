using UnityEngine;

public class PlayerWeaponPickup : MonoBehaviour
{
    [SerializeField] LaserWeapon laserWeapon;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerShoot>()?.EquipWeapon(laserWeapon);
        }
    }
}
