using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] TMP_Text healthText;
    [SerializeField] float flashDuration = 0.15f;
    [SerializeField] float flashInterval  = 0.05f;
    [SerializeField] PlayerMovement playerMovement;

    SpriteRenderer spriteRenderer;
    float currentHealth;
    float roundedHealth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage, Transform shooterPosition)
    {
        playerMovement.KnockBack(shooterPosition);
        currentHealth -= damage;
        roundedHealth = Mathf.Round(currentHealth * 100f) / 100f;

        StartCoroutine(InvincibilityFlash());
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
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
            healthText.text = roundedHealth.ToString();
        }
    }
}
