using UnityEngine;

public class EnemyCheckForPlayer : MonoBehaviour
{
    [SerializeField] float viewDistance;
    [SerializeField] Transform detectionPoint;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask obstacleLayer;

    public Transform player { get; private set; }

    void Update()
    {
        CheckForPlayer();
    }

    public bool CheckForPlayer()
    {
        Collider2D target = Physics2D.OverlapCircle(detectionPoint.position, viewDistance, playerLayer);

        // Check player and obstacle layer together
        LayerMask combinedMask = obstacleLayer | playerLayer;
        if (target != null)
        {
            Vector2 dirToTarget = (target.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, viewDistance, combinedMask);
            if (hit.collider == null || hit.collider == target)
            {
                player = target.transform;
                return true;
            }
            return false;
        }
        else
        {
            player = null;
            return false;
        }        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionPoint.position, viewDistance);
    }
}
