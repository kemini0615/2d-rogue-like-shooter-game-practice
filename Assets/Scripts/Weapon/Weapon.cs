using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    enum State
    {
        Idle,
        Attack,
    }

    State state;

    [SerializeField] Transform hitSpotTransform;
    [SerializeField] BoxCollider2D hitSpotCollider;
    [SerializeField] float hitSpotRadius;

    [SerializeField] LayerMask enemyMask;
    [SerializeField] float detectRange;
    [SerializeField] int damage;

    [SerializeField] float lerpMultiplier;

    [SerializeField] Animator animator;

    List<MeleeMonster> attackedEnemies = new List<MeleeMonster>();
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;

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
                UpdateIdle();
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }   
    }

    void StartIdle() {}

    void UpdateIdle()
    {
        IncreaseAttackTimer();
        AimAtClosestEnemy();
    }

    void ExitIdle() {}

    void StartAttack()
    {
        attackedEnemies.Clear();

        animator.speed = attackRate;
        animator.Play("Attack");
        state = State.Attack;
    }

    void UpdateAttack()
    {
        Attack();
    }
    
    // Attack 애니메이션이 끝나면 호출된다
    void ExitAttack()
    {
        state = State.Idle;
        attackedEnemies.Clear();
    }

    void IncreaseAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    private void AimAtClosestEnemy()
    {
        MeleeMonster closestEnemy = FindClosestEnemy();

        Vector2 targetVector;

        // 가장 가까운 적이 있다면
        if (closestEnemy != null)
        {
            // 바로 가장 가까운 적을 향한다
            targetVector = (closestEnemy.transform.position - transform.position).normalized;
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

    void TryAutoAttack()
    {
        if (attackTimer > attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(hitSpotTransform.position, hitSpotCollider.bounds.size, hitSpotTransform.localEulerAngles.z, enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            MeleeMonster targetEnemy = enemies[i].GetComponent<MeleeMonster>();
            if (!attackedEnemies.Contains(targetEnemy))
            {
                targetEnemy.TakeDamage(damage);
                attackedEnemies.Add(targetEnemy);
            }
        }
    }

    private MeleeMonster FindClosestEnemy()
    {
        MeleeMonster closestEnemy = null;

        // FindObjectsByType()를 Update() 메소드에서 프레임마다 호출하는 것은 권장하지 않는다
        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectRange, enemyMask);

        if (enemies.Length <= 0)
            return null;

        float minDistance = detectRange;

        for (int i = 0; i < enemies.Length; i++)
        {
            Collider2D enemy = enemies[i];
            float distance = Vector2.Distance(enemy.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.GetComponent<MeleeMonster>();
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireSphere(hitSpotTransform.position, hitSpotRadius);
    }
}
