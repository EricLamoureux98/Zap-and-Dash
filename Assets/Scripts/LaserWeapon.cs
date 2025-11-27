using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
    [SerializeField] float laserMaxDistance;
    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;    

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        Vector2 direction = transform.right;
        Vector2 endPoint; 

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, laserMaxDistance);

        if(hit.collider != null)
        {
            // hit.point returns where the raycast touches
            endPoint = hit.point; // If we hit something, the laser stops here

            // endPos = hit.transform.position; <--- this is wrong
            // This would return the transform (0,0) of what the raycast hit
        }
        else
        {
            // If we didn't the laser stops at max distance
            endPoint = (Vector2)firePoint.position + direction * laserMaxDistance;
        }

        Draw2DRay(firePoint.position, endPoint);
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
