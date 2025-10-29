using UnityEngine;
using UnityEngine.InputSystem;

public class AimTowardMouse : MonoBehaviour
{
    public Vector2 AimDirection { get; private set; }

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AimAtMouse();
    }
    
    void AimAtMouse()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        AimDirection = (mouseWorldPos - (Vector2)transform.position).normalized;

        
        anim.SetFloat("aimVertical", AimDirection.y);        
    }
}
