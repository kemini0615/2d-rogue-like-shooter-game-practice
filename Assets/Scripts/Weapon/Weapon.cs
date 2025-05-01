using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform hitSpotTransform;
    [SerializeField] BoxCollider2D hitSpotCollider;
    [SerializeField] Animator animator;

    [Header("Attack")]
    [SerializeField] LayerMask monsterMask;
    [SerializeField] float lerpMultiplier;
    [SerializeField] float detectRange;
    [SerializeField] int damage;
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;
    List<Monster> attackedMonsters = new List<Monster>();
    
    enum State
    {
        Idle,
        Attack,
    }

    State state;

    void Start()
    {
        state = State.Idle;
        attackDelay = 1f / attackRate;

        hitSpotCollider = hitSpotTransform.GetComponent<BoxCollider2D>();
    }

    void Update()
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

    void StartIdleState() {}

    void UpdateIdleState()
    {
        IncreaseAttackTimer();
        AimAtClosestMonster();
    }

    void ExitIdleState() {}

    void StartAttackState()
    {
        attackedMonsters.Clear();

        animator.speed = attackRate;
        animator.Play("Attack");
        state = State.Attack;
    }

    void UpdateAttackState()
    {
        Attack();
    }
    
    // Attack 애니메이션이 끝나면 호출된다
    void ExitAttackState()
    {
        state = State.Idle;
        attackedMonsters.Clear();
    }

    void IncreaseAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    private void AimAtClosestMonster()
    {
        Monster closestMonster = FindClosestMonster();

        Vector2 targetVector;

        // 가장 가까운 적이 있다면
        if (closestMonster != null)
        {
            // 바로 가장 가까운 적을 향한다
            targetVector = (closestMonster.transform.position - transform.position).normalized;
            transform.up = targetVector;
            
            // 공격을 시도한다
            TryAutoAttack();
        }
        else
        {
            // 가장 가까운 적이 없다면 천천히 위쪽 방향을 향한다
            targetVector = Vector2.up;
            transform.up = Vector2.Lerp(transform.up, targetVector, Time.deltaTime * lerpMultiplier);
        }
    }

    private Monster FindClosestMonster()
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

    void TryAutoAttack()
    {
        if (attackTimer > attackDelay)
        {
            attackTimer = 0;
            StartAttackState();
        }
    }

    private void Attack()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
