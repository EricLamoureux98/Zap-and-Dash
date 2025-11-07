using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyPatrol enemyPatrol;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] EnemyCheckForPlayer checkForPlayer;

    Rigidbody2D rb;
    Animator anim;
    EnemyState enemyState;

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
            ChangeState(EnemyState.Attacking);
        }
        else if (!seesPlayer)
        {
            ChangeState(EnemyState.Patrolling);
        }
    }

    void ChangeState(EnemyState newState)
    {
        enemyState = newState;

        if (newState == EnemyState.Patrolling)
        {
            enemyPatrol.enabled = true;
            enemyAttack.enabled = false;
            anim.SetBool("isShooting", false);
            //anim.SetBool("isWalking", true);
        }

        if (newState == EnemyState.Attacking)
        {
            enemyPatrol.enabled = false;
            enemyAttack.enabled = true;
            rb.linearVelocity = Vector2.zero;
            //anim.SetBool("isWalking", false);
            anim.SetBool("isShooting", true);
        }
    }
}

public enum EnemyState
{
    Patrolling,
    Attacking
}
