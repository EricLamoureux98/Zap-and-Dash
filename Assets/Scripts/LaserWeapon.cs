using UnityEngine;

public class LaserWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] float laserMaxDistance;
    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;    
    [SerializeField] LayerMask obstacleLayer;

    //[SerializeField] AimTowardMouse aimTowardMouse;
    //[SerializeField] PlayerShoot playerShoot;

    Vector2 direction;
    bool isFiring = false;

    void Start()
    {
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!isFiring)
        {
            lineRenderer.enabled = false;
        }
    }

    public void Fire()
    {
        isFiring = true;
        ShootLaser();
        lineRenderer.enabled = true;
    }

    void ShootLaser()
    {
        direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        //Vector2 direction = transform.right;
        //Vector2 direction = aimTowardMouse.AimDirection; // <--- Feels weird
        Vector2 endPoint; 

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, laserMaxDistance, obstacleLayer);

        if(hit.collider != null)
        {
            // hit.point returns where the raycast touches
            endPoint = hit.point; // If we hit something, the laser stops here

            // endPos = hit.transform.position; <--- this is wrong
            // This would return the transform (0,0) of what the raycast hit
        }
        else
        {
            // If we didn't hit, the laser stops at max distance
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
