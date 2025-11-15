using System.Collections;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] TMP_Text healthText;
    [SerializeField] float flashDuration = 0.15f;
    [SerializeField] float flashInterval  = 0.05f;

    SpriteRenderer spriteRenderer;
    float currentHealth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(InvincibilityFlash());
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }

    IEnumerator InvincibilityFlash()
    {
        float timer = 0f;
        while (timer < flashDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

    }

    void UpdateHealthUI()
    {
        if (healthText)
        {
            healthText.text = currentHealth.ToString();
        }
    }
}
