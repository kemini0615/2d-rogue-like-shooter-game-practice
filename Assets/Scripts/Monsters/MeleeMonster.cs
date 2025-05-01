using UnityEngine;

public class MeleeMonster : Monster
{
    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;

    protected override void Start()
    {
        base.Start();

        attackDelay = 1f / attackRate; 
    }

    protected void Update()
    {
        if (!CanFollowPlayer())
            return;

        monsterMovement.FollowPlayer();

        attackTimer += Time.deltaTime;
        TryAttack();
    }

    void TryAttack()
    {
        float distToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
