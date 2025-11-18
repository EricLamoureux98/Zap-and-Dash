using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy AI")]
    [SerializeField] EnemyPatrol enemyPatrol;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] EnemyCheckForPlayer checkForPlayer;

    [Header("Enemy Stats")]
    [SerializeField] float playerSeenTime = 0.5f;
    [SerializeField] float continueFiringTime;

    Rigidbody2D rb;
    Animator anim;
    EnemyState enemyState;
    Transform playerPosition;
    Transform lastPlayerPosition;
    Transform shooterTarget; // Direction of player if they shoot enemy
    Transform targetToAttack; 
    float seenPlayerTimer;
    float continueFiringTimer;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        ChangeState(EnemyState.Patrolling);
        shooterTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //Debug.Log(enemyState.ToString());
        if (enemyState == EnemyState.Dead) return;
        
        bool seesPlayer = checkForPlayer.CheckForPlayer();        
        playerPosition = checkForPlayer.player;

        if (seesPlayer)
        {
            lastPlayerPosition = playerPosition;
            seenPlayerTimer += Time.deltaTime;
            continueFiringTimer = 0f;
            ChangeState(EnemyState.PlayerDetected);

            if (seenPlayerTimer >= playerSeenTime)
            {
                ChangeState(EnemyState.Attacking);
            }
        }                                                    // Prevents null error at startup
        else if (continueFiringTimer < continueFiringTime && lastPlayerPosition != null)
        {
            continueFiringTimer += Time.deltaTime;
            playerPosition = lastPlayerPosition;
            ChangeState(EnemyState.Attacking);          
        }
        else
        {
            seenPlayerTimer = 0f;
            ChangeState(EnemyState.Patrolling);
        }

        if (enemyState == EnemyState.Attacking)
        {
            enemyAttack.ShootAtTarget(playerPosition);
        }        
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
        if (playerPosition != null)
        {
            targetToAttack = playerPosition;
        }
        else
        {
            targetToAttack = shooterTarget;
        }

        float directionToTarget = targetToAttack.position.x - transform.position.x;

        if (rb.linearVelocity.x > 0 && directionToTarget < 0 || rb.linearVelocity.x < 0 && directionToTarget > 0 )
        {
            Flip();
        }

        enemyPatrol.SetActive(false);
        enemyAttack.SetActive(true);
        anim.SetBool("isWalking", false);
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isShooting", true);
        enemyAttack.ShootAtTarget(targetToAttack);
    }

    void HandleDying()
    {
        enemyPatrol.SetActive(false);
        enemyAttack.SetActive(false); 
        anim.SetBool("isDead", true);
        this.enabled = false;
        Destroy(gameObject, 5f);
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

        if (newState == EnemyState.Dead)
        {
            HandleDying();
        }
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
}

public enum EnemyState
{
    Patrolling,
    PlayerDetected,
    Attacking,
    Dead
}
