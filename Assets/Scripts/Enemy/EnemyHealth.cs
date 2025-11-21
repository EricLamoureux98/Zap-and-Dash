using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float flashDuration = 0.15f;
    [SerializeField] float flashInterval  = 0.05f;
    [SerializeField] EnemyController controller;

    SpriteRenderer spriteRenderer;
    float currentHealth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, Transform shooterPosition)
    {
        controller.AlertToPlayer(shooterPosition);
        currentHealth -= damage;
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            //Destroy(gameObject);
            controller.Die();
        }
    }

    IEnumerator DamageFlash()
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
}
