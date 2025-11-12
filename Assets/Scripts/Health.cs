using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;

    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }
}
