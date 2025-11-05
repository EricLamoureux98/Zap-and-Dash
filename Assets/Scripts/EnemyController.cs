using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyPatrol enemyPatrol;
    [SerializeField] EnemyAttack enemyAttack;

    public bool isPatrolling;
    public bool isAttacking = false;

    Rigidbody2D rb;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        //isPatrolling = true;
    }

    void Update()
    {
        if (isPatrolling)
        {
            enemyPatrol.enabled = true;
        }
        else if(!isPatrolling)
        {
            enemyPatrol.enabled = false;
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isWalking", false);
        }
    }
}
