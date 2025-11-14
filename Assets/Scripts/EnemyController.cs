using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyPatrol enemyPatrol;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] EnemyCheckForPlayer checkForPlayer;
    [SerializeField] float playerSeenTime = 0.5f;

    Rigidbody2D rb;
    Animator anim;
    EnemyState enemyState;
    float seenPlayerTimer;
    

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
            seenPlayerTimer += Time.deltaTime;
            ChangeState(EnemyState.PlayerDetected);
        }        

        if (seesPlayer && seenPlayerTimer >= playerSeenTime)
        {
            ChangeState(EnemyState.Attacking);
        }
        else if (!seesPlayer)
        {
            seenPlayerTimer = 0;
            ChangeState(EnemyState.Patrolling);
        }
    }

    void ChangeState(EnemyState newState)
    {
        if (newState == enemyState) return;

        enemyState = newState;

        if (newState == EnemyState.Patrolling)
        {
            enemyPatrol.SetActive(true);
            enemyAttack.SetActive(false);
            anim.SetBool("isShooting", false);
            //anim.SetBool("isWalking", true);
        }

        if (newState == EnemyState.PlayerDetected)
        {
            enemyPatrol.SetActive(false);
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isWalking", false);
        }

        if (newState == EnemyState.Attacking)
        {
            enemyPatrol.SetActive(false);
            enemyAttack.SetActive(true);
            rb.linearVelocity = Vector2.zero;
            //anim.SetBool("isWalking", false);
            anim.SetBool("isShooting", true);
        }
    }
}

public enum EnemyState
{
    Patrolling,
    PlayerDetected,
    Attacking
}
