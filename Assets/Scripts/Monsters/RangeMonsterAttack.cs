using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Pool;

public class RangeMonsterAttack : MonoBehaviour
{   
    [SerializeField] MonsterBullet bulletPrefab;
    [SerializeField] Transform shootingPoint;

    // [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;

    ObjectPool<MonsterBullet> bulletPool;

    void Start()
    {
        attackDelay = 1f / attackRate; 
        attackTimer = attackDelay; // 시작하자마자 공격 가능

        bulletPool = new ObjectPool<MonsterBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet);
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
    }

    MonsterBullet CreateBullet()
    {
        return Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
    }

    void OnGetBullet(MonsterBullet bullet)
    {  
        bullet.gameObject.SetActive(true);
        bullet.Init(shootingPoint.position);
    }

    void OnReleaseBullet(MonsterBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyBullet(MonsterBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void TryAttack(float attackRange, float distToPlayer)
    {
        if (attackTimer < attackDelay)
            return;

        // 플레이어가 공격 범위 안에 있으면 공격한다
        if (distToPlayer <= attackRange)
            RangeAttack();
    }

    // void Attack()
    // {
    //     attackTimer = 0f;

    //     Player.Instance.TakeDamage(attackDamage);
    // }

    void RangeAttack()
    {
        Vector2 direction = (Player.Instance.GetCenterPosition() - (Vector2) shootingPoint.position).normalized;
        ShootBullet(direction);
        attackTimer = 0f;
    }

    void ShootBullet(Vector2 direction)
    {
        MonsterBullet bullet = bulletPool.Get();
        bullet.bulletExpired -= ReleaseBullet;
        bullet.bulletExpired += ReleaseBullet;
        bullet.Shoot(direction, attackDamage);
    }

    // 오브젝트 풀에 총알을 반납하는 콜백함수
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
    }
}
