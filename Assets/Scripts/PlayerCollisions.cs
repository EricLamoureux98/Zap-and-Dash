using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] Health health;
    //[SerializeField] CompositeCollider2D waterCollider;

    void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Water"))
        {
            health.TakeDamage(999);
        }
    }
}
