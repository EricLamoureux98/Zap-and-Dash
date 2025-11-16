using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] EnemyCheckForPlayer checkForPlayer;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float speed;
    [SerializeField] float arriveThreshold; // How close to point
    [SerializeField] float pauseAtPointTime; // How long to pause at point

    Rigidbody2D rb;
    Animator anim;

    bool canPatrol = true;
    Transform currentPoint;
    float pauseTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentPoint = pointB;
    }

    void FixedUpdate()
    {
        if (!canPatrol)
        {
            Debug.Log("Patrol is disabled");
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
            DoPatrol();
            Debug.Log("Patrol running");
    }

    public void SetActive(bool active)
    {
        canPatrol = active;

        if (!active)
        {
            CancelInvoke(); // Cancel any pending flips
        }
    }

    void DoPatrol()
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.fixedDeltaTime;
            anim.SetBool("isWalking", false);
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (currentPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
        anim.SetBool("isWalking", true);

        if (Vector2.Distance(transform.position, currentPoint.position) <= arriveThreshold)
        {
            currentPoint = (currentPoint == pointA) ? pointB : pointA;
            pauseTimer = pauseAtPointTime;
            Invoke("Flip", pauseAtPointTime); // Careful with Invoke. Will run even if script is disabled - Coroutine would avoid this
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, arriveThreshold);
        Gizmos.DrawWireSphere(pointB.transform.position, arriveThreshold);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
