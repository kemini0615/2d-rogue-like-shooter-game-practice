using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float speed;
    int attackDamage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction, int attackDamage)
    {
        transform.right = direction;
        // Rigidbody.velocity는 더이상 사용하지 않는다
        rb.linearVelocity = direction * speed;
        this.attackDamage = attackDamage;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // 충돌한 상대가 플레이어라면 공격
        if (otherCollider.TryGetComponent(out Player player))
        {
            player.TakeDamage(this.attackDamage);
            Destroy(gameObject);
        }
    }
}
