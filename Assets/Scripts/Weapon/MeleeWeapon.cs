using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Components")]
    [SerializeField] Transform hitSpotTransform;
    [SerializeField] BoxCollider2D hitSpotCollider;
    [SerializeField] protected Animator animator;

    protected override void Start()
    {
        base.Start();

        hitSpotCollider = hitSpotTransform.GetComponent<BoxCollider2D>();
    }

    protected override void StartIdleState()
    {
        throw new System.NotImplementedException();
    }
    
    protected override void UpdateIdleState()
    {
        IncreaseAttackTimer();
        AimAtClosestMonster();
    }

    protected override void ExitIdleState()
    {
        throw new System.NotImplementedException();
    }
    
    protected override void StartAttackState()
    {
        attackedMonsters.Clear();

        animator.speed = attackRate;
        animator.Play("Attack");
        state = State.Attack;
    }

    protected override void UpdateAttackState()
    {
        MeleeAttack();
    }
    
    // Attack 애니메이션이 끝나면 호출된다
    protected override void ExitAttackState()
    {
        state = State.Idle;
        attackedMonsters.Clear();
    }

    // 조건이 충족되면 공격한다
    protected override void TryAutoAttack()
    {
        if (attackTimer < attackDelay)
            return;

        attackTimer = 0;
        StartAttackState();
    }

    protected void MeleeAttack()
    {
        Collider2D[] monsterColliders = Physics2D.OverlapBoxAll(hitSpotTransform.position, hitSpotCollider.bounds.size, hitSpotTransform.localEulerAngles.z, monsterMask);

        for (int i = 0; i < monsterColliders.Length; i++)
        {
            Monster targetMonster = monsterColliders[i].GetComponent<Monster>();
            if (!attackedMonsters.Contains(targetMonster))
            {
                targetMonster.TakeDamage(damage);
                attackedMonsters.Add(targetMonster);
            }
        }
    }
}
