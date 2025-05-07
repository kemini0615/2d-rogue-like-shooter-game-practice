using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Components")]
    // [SerializeField] Transform hitSpotTransform;
    // [SerializeField] BoxCollider2D hitSpotCollider;
    [SerializeField] Animator animator;

    [Header("Attack")]
    [SerializeField] protected LayerMask monsterMask;
    [SerializeField] protected float lerpMultiplier;
    [SerializeField] protected float detectRange;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackRate;
    protected float attackDelay;
    protected float attackTimer;
    protected List<Monster> attackedMonsters = new List<Monster>();
    
    protected enum State
    {
        Idle,
        Attack,
    }

    protected State state;

    protected virtual void Start()
    {
        state = State.Idle;
        attackDelay = 1f / attackRate;
    }

    protected void Update()
    {
        // State machine pattern
        switch (state)
        {
            case State.Idle:
                UpdateIdleState();
                break;
            case State.Attack:
                UpdateAttackState();
                break;
        }   
    }

    protected void UpdateIdleState()
    {
        IncreaseAttackTimer();
        AimAtClosestMonster();
    }

    protected void StartAttackState()
    {
        attackedMonsters.Clear();

        animator.speed = attackRate;
        animator.Play("Attack");
        state = State.Attack;
    }

    protected void UpdateAttackState()
    {
        Attack();
    }
    
    // Attack 애니메이션이 끝나면 호출된다
    protected void ExitAttackState()
    {
        state = State.Idle;
        attackedMonsters.Clear();
    }

    protected void IncreaseAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    protected void AimAtClosestMonster()
    {
        Monster closestMonster = FindClosestMonster();

        Vector2 targetVector;

        // 가장 가까운 적이 있다면
        if (closestMonster != null)
        {
            // 바로 가장 가까운 적을 향한다
            targetVector = (closestMonster.transform.position - transform.position).normalized;
            transform.up = targetVector;
        }
        else
        {
            // 가장 가까운 적이 없다면 천천히 위쪽 방향을 향한다
            targetVector = Vector2.up;
            transform.up = Vector2.Lerp(transform.up, targetVector, Time.deltaTime * lerpMultiplier);
            return;
        }

        // 공격을 시도한다
        TryAttack();
    }

    protected Monster FindClosestMonster()
    {
        Monster closestMonster = null;

        // FindObjectsByType()를 Update() 메소드에서 프레임마다 호출하는 것은 권장하지 않는다
        // Monster[] monsters = FindObjectsByType<Monster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Collider2D[] monsterColliders = Physics2D.OverlapCircleAll(transform.position, detectRange, monsterMask);

        if (monsterColliders.Length <= 0)
            return null;

        float minDistance = detectRange;

        for (int i = 0; i < monsterColliders.Length; i++)
        {
            Collider2D monsterCollider = monsterColliders[i];
            float distance = Vector2.Distance(monsterCollider.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestMonster = monsterCollider.GetComponent<Monster>();
            }
        }

        return closestMonster;
    }

    // 조건이 충족되면 공격한다
    protected abstract void TryAttack();

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
