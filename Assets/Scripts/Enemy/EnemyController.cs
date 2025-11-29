using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy AI")]
    [SerializeField] EnemyPatrol enemyPatrol;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] EnemyCheckForPlayer checkForPlayer;

    [Header("Enemy Stats")]
    [SerializeField] EnemySO enemySO;
    [SerializeField] float playerSeenTime = 0.5f;
    [SerializeField] float continueFiringTime;

    [Header("Knockback")]
    [SerializeField] float knockbackForce = 10f;
    [SerializeField] float stunTime = 0.5f;

    Rigidbody2D rb;
    Animator anim;
    EnemyState enemyState;
    Transform playerPosition;
    Transform lastPlayerPosition;
    //Transform targetToAttack; 
    float seenPlayerTimer;
    float continueFiringTimer;
    bool seesPlayer;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        ChangeState(EnemyState.Patrolling);
    }

    void Update()
    {
        //Debug.Log(enemyState.ToString());
        if (enemyState == EnemyState.Dead) return;
        if (enemyState == EnemyState.KnockedBack) return;

        UpdateTimers();
        UpdatePlayerDetection();
        DetermineState();
 
        if (enemyState == EnemyState.Attacking)
        {
            enemyAttack.ShootAtTarget(playerPosition);
        }        
    }

    void UpdateTimers()
    {
        if (seesPlayer)
        {
            seenPlayerTimer += Time.deltaTime;
            continueFiringTimer = 0f;
        }
        else if (ShouldContinueFiring())
        {
            continueFiringTimer += Time.deltaTime;
        }
        else
        {
            seenPlayerTimer = 0f; 
        }
    }

    void UpdatePlayerDetection()
    {
        seesPlayer = checkForPlayer.CheckForPlayer();        
        playerPosition = checkForPlayer.player;

        if (seesPlayer)
        {
            lastPlayerPosition = playerPosition;
        }
        else if (ShouldContinueFiring())
        {            
            playerPosition = lastPlayerPosition;                     
        }
    }

    void DetermineState()
    {
        if (seesPlayer)
        {
            ChangeState(EnemyState.PlayerDetected);
            if (seenPlayerTimer >= playerSeenTime)
            {
                ChangeState(EnemyState.Attacking);
            }
        }        
        else if (ShouldContinueFiring())
        {
            ChangeState(EnemyState.Attacking); 
        }
        else
        {
            ChangeState(EnemyState.Patrolling);
        }
    }

    Transform GetTargetToAttack()
    {
        if (playerPosition != null)
        {
            return playerPosition;
        }
        else
        {
            return lastPlayerPosition;
        }
    }

    bool ShouldContinueFiring()
    {
        return continueFiringTimer < continueFiringTime && lastPlayerPosition != null;
    }

    void HandlePatrol()
    {
        enemyPatrol.SetActive(true);
        enemyAttack.SetActive(false);
        anim.SetBool("isShooting", false);
    }

    void HandleDetected()
    {
        enemyPatrol.SetActive(false);
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isWalking", false);
    }

    void HandleAttack()
    {
        Transform targetToAttack = GetTargetToAttack();

        float directionToTarget = targetToAttack.position.x - transform.position.x;
        if (transform.localScale.x > 0 && directionToTarget < 0 || transform.localScale.x < 0 && directionToTarget > 0 )
        {
            Flip();
        }

        enemyPatrol.SetActive(false);
        enemyAttack.SetActive(true);
        enemyAttack.damage = enemySO.damage;
        anim.SetBool("isWalking", false);
        anim.SetBool("isShooting", true);
    }

    void HandleKnockback()
    {
        enemyPatrol.SetActive(false);
        enemyAttack.SetActive(false);         
    }

    void HandleDying()
    {
        enemyPatrol.SetActive(false);
        enemyAttack.SetActive(false); 
        anim.SetBool("isDead", true);
        this.enabled = false;
        Destroy(gameObject, 5f);
    }    

    public void Die()
    {
        ChangeState(EnemyState.Dead);
    }

    public void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void AlertToPlayer(Transform playerTransform)
    {
        continueFiringTimer = 0f;
        lastPlayerPosition = playerTransform;
        ChangeState(EnemyState.Attacking);
    }

    public void KnockBack(Transform shooterPosition)
    {
        ChangeState(EnemyState.KnockedBack);
        Vector2 direction = (transform.position - shooterPosition.position).normalized;
        Vector2 force = knockbackForce * direction;
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(StunTimer());
    }

    IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        //ChangeState(EnemyState.Attacking);
    }

    public void ChangeState(EnemyState newState)
    {
        if (newState == enemyState) return;

        enemyState = newState;

        if (newState == EnemyState.Patrolling)
        {
            HandlePatrol();
        }

        if (newState == EnemyState.PlayerDetected)
        {
            HandleDetected();
        }

        if (newState == EnemyState.Attacking)
        {
            HandleAttack();
        }

        if (newState == EnemyState.KnockedBack)
        {
            HandleKnockback();
        }

        if (newState == EnemyState.Dead)
        {
            HandleDying();
        }
    }
}

public enum EnemyState
{
    Patrolling,
    PlayerDetected,
    Attacking,
    KnockedBack,
    Dead
}
