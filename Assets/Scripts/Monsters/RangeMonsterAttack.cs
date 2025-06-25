using UnityEngine;
using UnityEngine.Pool;

public class RangeMonsterAttack : MonoBehaviour
{   
    [SerializeField] MonsterBullet bulletPrefab;
    [SerializeField] Transform shootingPoint;

    [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;

    ObjectPool<MonsterBullet> bulletPool;

    void Start()
    {
        attackDelay = 1f / attackRate; 
        attackTimer = attackDelay; // 시작하자마자 공격 가능.

        bulletPool = new ObjectPool<MonsterBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet);
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public void TryAttack()
    {
        if (attackTimer < attackDelay)
            return;

        float distToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

        // 플레이어가 공격 범위 안에 있으면 공격한다.
        if (distToPlayer <= attackRange)
            RangeAttack();
    }

    void RangeAttack()
    {
        attackTimer = 0f;

        Vector2 direction = (Player.Instance.GetCenterPosition() - (Vector2) shootingPoint.position).normalized;
        ShootBullet(direction);
    }

    void ShootBullet(Vector2 direction)
    {
        MonsterBullet bullet = bulletPool.Get();
        bullet.expired -= ReleaseBullet;
        bullet.expired += ReleaseBullet;
        bullet.Shoot(direction, attackDamage);
    }

    // 오브젝트 풀에 오브젝트가 없을 때, 오브젝트를 생성하는 함수.
    MonsterBullet CreateBullet()
    {
        return Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
    }

    // 오브젝트 풀에서 오브젝트를 가져올 때마다(Get() 메소드를 호출할 때마다) 호출되는 액션 함수.
    void OnGetBullet(MonsterBullet bullet)
    {  
        bullet.gameObject.SetActive(true);
        bullet.Init(shootingPoint.position);
    }

    // 오브젝트 풀에 오브젝트를 반납할 때마다(Release() 메소드를 호출할 때마다) 호출되는 액션 함수.
    void OnReleaseBullet(MonsterBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    // 오브젝트 풀이 파괴되거나, 더이상 풀에 오브젝트를 저장할 수 없을 때 호출되는 액션 함수.
    void OnDestroyBullet(MonsterBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(MonsterBullet bullet)
    {
        bulletPool.Release(bullet);
    }

    void OnDrawGizmos()
    {
        if (!Player.Instance)
            return;

        Vector2 direction = Player.Instance.GetCenterPosition() - (Vector2) shootingPoint.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(shootingPoint.position, (Vector2) shootingPoint.position + direction);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
