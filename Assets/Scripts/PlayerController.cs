using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] MobileJoystick joystick;
    [SerializeField] float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = joystick.GetMoveVector() * moveSpeed * Time.fixedDeltaTime;
    }
}
