using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] protected LayerMask monsterMask;
    [SerializeField] protected float lerpMultiplier;
    [SerializeField] protected float detectRange;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackRate;
    protected float attackDelay;
    protected float attackTimer;
    
    protected virtual void Start()
    {
        attackDelay = 1f / attackRate;
    }

    protected abstract void Update();

    protected void IncreaseAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    protected Monster AimAtClosestMonster()
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

        return closestMonster;
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

    protected void TryAutoAttack()
    {
        if (attackTimer < attackDelay)
            return;

        Attack();
        attackTimer = 0f;
    }

    protected abstract void Attack();

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
