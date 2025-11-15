using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyPatrol enemyPatrol;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] EnemyCheckForPlayer checkForPlayer;
    [SerializeField] float playerSeenTime = 0.5f;
    [SerializeField] float continueFiringTime;

    Rigidbody2D rb;
    Animator anim;
    EnemyState enemyState;
    Transform targetPosition;
    Transform lastPlayerPosition;
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
    }

    void Update()
    {
        bool seesPlayer = checkForPlayer.CheckForPlayer();        

        if (seesPlayer)
        {
            targetPosition = checkForPlayer.player;
            lastPlayerPosition = targetPosition;
            seenPlayerTimer += Time.deltaTime;
            continueFiringTimer = 0f;
            ChangeState(EnemyState.PlayerDetected);

            if (seenPlayerTimer >= playerSeenTime)
            {
                ChangeState(EnemyState.Attacking);
            }
        }  
        else
        {
            continueFiringTimer += Time.deltaTime;
            targetPosition = lastPlayerPosition;

            if (continueFiringTimer >= continueFiringTime)
            {
                seenPlayerTimer = 0f;
                ChangeState(EnemyState.Patrolling);
            }
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
        enemyPatrol.SetActive(false);
        enemyAttack.SetActive(true);
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isShooting", true);
        enemyAttack.ShootAtTarget(targetPosition);
    }

    void ChangeState(EnemyState newState)
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
    }
}

public enum EnemyState
{
    Patrolling,
    PlayerDetected,
    Attacking
}
