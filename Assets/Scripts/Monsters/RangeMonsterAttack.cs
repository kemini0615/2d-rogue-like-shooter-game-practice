using Unity.Mathematics;
using UnityEngine;

public class RangeMonsterAttack : MonoBehaviour
{   
    [SerializeField] Transform shootingPoint;
    [SerializeField] GameObject bulletPrefab;

    // [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;

    void Start()
    {
        attackDelay = 1f / attackRate; 
        attackTimer = attackDelay; // 시작하자마자 공격 가능
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public void TryAttack(float attackRange, float distToPlayer)
    {
        if (attackTimer < attackDelay)
            return;

        // 플레이어가 공격 범위 안에 있으면 공격한다
        if (distToPlayer <= attackRange)
            RangeAttack();
    }

    void Attack()
    {
        attackTimer = 0f;

        Player.Instance.TakeDamage(attackDamage);
    }

    void RangeAttack()
    {
        Vector2 direction = (Player.Instance.GetCenterPosition() - (Vector2) shootingPoint.position).normalized;
        Shoot(direction);
        attackTimer = 0f;
    }

    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, quaternion.identity);
        bullet.transform.right = direction;
        // * Rigidbody.velocity는 더이상 사용하지 않는다 *
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10;
    }

    void OnDrawGizmos()
    {
        if (!Player.Instance)
            return;

        Vector2 direction = Player.Instance.GetCenterPosition() - (Vector2) shootingPoint.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(shootingPoint.position, (Vector2) shootingPoint.position + direction);
    }
}
