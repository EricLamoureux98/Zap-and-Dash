using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] TMP_Text healthText;

    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }

    void UpdateHealthUI()
    {
        if (healthText)
        {
            healthText.text = currentHealth.ToString();
        }
    }
}
