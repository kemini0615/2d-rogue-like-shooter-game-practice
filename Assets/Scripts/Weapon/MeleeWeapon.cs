using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Components")]
    [SerializeField] Transform hitSpotTransform;
    [SerializeField] BoxCollider2D hitSpotCollider;
    [SerializeField] protected Animator animator;

    protected List<Monster> attackedMonsters = new List<Monster>();

    // 상태 머신 패턴.
    protected enum State
    {
        Idle,
        Attack,
    }

    protected State currentState;

    protected override void Start()
    {
        base.Start();

        currentState = State.Idle;
        hitSpotCollider = hitSpotTransform.GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdleState();
                break;
            case State.Attack:
                UpdateAttackState();
                break;
        }      
    }

    protected override void Attack()
    {
        base.Attack();
        EnterAttackState();
    }
    
    protected void UpdateIdleState()
    {
        IncreaseAttackTimer();
        AimAtClosestMonster();
    }
    
    protected void EnterAttackState()
    {
        attackedMonsters.Clear();

        // 애니메이션 재생.
        animator.speed = attackRate;
        animator.Play("Attack");
        currentState = State.Attack;
    }

    protected void UpdateAttackState()
    {
        MeleeAttack();
    }
    
    // Attack 애니메이션이 끝나면 호출된다.
    protected void ExitAttackState()
    {
        currentState = State.Idle;
        attackedMonsters.Clear();
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
