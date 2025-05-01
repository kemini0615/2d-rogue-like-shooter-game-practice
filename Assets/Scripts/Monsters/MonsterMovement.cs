using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float targetDistance;

    public void FacePlayer()
    {
        bool facingRight = Player.Instance.transform.position.x > transform.position.x;
        transform.localScale = facingRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }

    public void FollowPlayer()
    {
        float distToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);
        if (distToPlayer < targetDistance)
            return;

        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        transform.position = (Vector2) transform.position + direction * moveSpeed * Time.deltaTime;
    }
}
