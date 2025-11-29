using UnityEngine;

public class PlayerWeaponPickup : MonoBehaviour
{
    LaserWeapon laserWeapon;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Equpped laser weapon");
            collision.GetComponent<PlayerShoot>()?.EquipWeapon(laserWeapon);
        }
    }
}
