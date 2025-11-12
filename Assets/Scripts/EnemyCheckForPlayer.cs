using UnityEngine;

public class EnemyCheckForPlayer : MonoBehaviour
{
    [SerializeField] float viewDistance;
    [SerializeField] Transform detectionPoint;
    [SerializeField] LayerMask playerLayer;

    public Transform player { get; private set; }

    void Update()
    {
        CheckForPlayer();
    }

    public bool CheckForPlayer()
    {
        Collider2D target = Physics2D.OverlapCircle(detectionPoint.position, viewDistance, playerLayer);

        if (target != null)
        {
            player = target.transform;
            return true;
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
