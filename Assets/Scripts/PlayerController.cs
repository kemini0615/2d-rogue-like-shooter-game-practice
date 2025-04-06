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
        Vector2 keyboardInputMoveVector = GetMoveVectorByKeyboard();

        if (keyboardInputMoveVector != Vector2.zero)
            rb.linearVelocity = keyboardInputMoveVector * moveSpeed * Time.fixedDeltaTime;
        else
            rb.linearVelocity = joystick.GetMoveVector() * moveSpeed * Time.fixedDeltaTime;
    }

    Vector2 GetMoveVectorByKeyboard()
    {
        const float MoveSpeedMultiplier = 100f;

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        return new Vector2(xInput, yInput) * MoveSpeedMultiplier;
    }
}
