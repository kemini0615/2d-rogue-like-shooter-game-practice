using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header("Components")]
    [SerializeField] Transform shootingPoint;
    [SerializeField] Bullet bulletPrefab;

    ObjectPool<Bullet> bulletPool;

    Monster targetMonster;

    protected override void Start()
    {
        base.Start();

        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet);
    }

    protected override void Update()
    {
        IncreaseAttackTimer();
        targetMonster = AimAtClosestMonster();
    }

    protected override void Attack()
    {
        RangeAttack();
    }

    protected void RangeAttack()
    {
        if (targetMonster == null)
            return;

        Vector2 direction = (targetMonster.GetCenterPosition() - (Vector2) shootingPoint.position).normalized;
        ShootBullet(direction);
    }

    void ShootBullet(Vector2 direction)
    {
        Bullet bullet = bulletPool.Get();
        bullet.bulletExpired -= ReleaseBullet;
        bullet.bulletExpired += ReleaseBullet;
        bullet.Shoot(direction, damage);
    }

    Bullet CreateBullet()
    {
        return Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
    }

    void OnGetBullet(Bullet bullet)
    {  
        bullet.gameObject.SetActive(true);
        bullet.Init(shootingPoint.position);
    }

    void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    // 오브젝트 풀에 총알을 반납하는 콜백함수
    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }
}
