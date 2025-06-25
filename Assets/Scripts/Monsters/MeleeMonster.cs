using UnityEngine;

// MeleeMonster는 '공격' 기능이 단순하기 때문에 코드를 분리하지 않았다.
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

        // 플레이어를 추적한다.
        monsterMovement.FollowPlayer();

        attackTimer += Time.deltaTime;

        // 공격을 시도한다.
        TryAttack();
    }

    void TryAttack()
    {
        if (attackTimer < attackDelay)
            return;

        float distToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

        // 플레이어가 공격 범위 안에 있으면 공격한다.
        if (distToPlayer <= attackRange)
            MeleeAttack();
    }

    void MeleeAttack()
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
