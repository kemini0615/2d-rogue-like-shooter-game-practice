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
            Attack();
    }

    void Attack()
    {
        attackTimer = 0f;

        Player.Instance.TakeDamage(attackDamage);
    }
}
