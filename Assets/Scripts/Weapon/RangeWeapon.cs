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

    protected override void StartIdleState()
    {
        throw new System.NotImplementedException();
    }
    
    protected override void UpdateIdleState()
    {
        IncreaseAttackTimer();
        this.targetMonster = AimAtClosestMonster();
    }

    protected override void ExitIdleState()
    {
        throw new System.NotImplementedException();
    }
    

    protected override void StartAttackState()
    {
        attackedMonsters.Clear();
        state = State.Attack;
    }

    protected override void UpdateAttackState()
    {
        RangeAttack();
    }
    
    // TODO
    protected override void ExitAttackState()
    {
        state = State.Idle;
        attackedMonsters.Clear();
    }

    // 조건이 충족되면 공격한다
    protected override void TryAutoAttack()
    {
        // 조건 1: 공격 타이머
        if (attackTimer < attackDelay)
            return;

        // 조건 2: 공격 사정거리
        // TODO

        attackTimer = 0;
        StartAttackState();
    }

    protected void RangeAttack()
    {
        if (this.targetMonster == null)
            return;

        Vector2 direction = (this.targetMonster.GetCenterPosition() - (Vector2) shootingPoint.position).normalized;
        ShootBullet(direction);
        attackTimer = 0f;

        ExitAttackState();
    }

    void ShootBullet(Vector2 direction)
    {
        Bullet bullet = bulletPool.Get();
        bullet.bulletExpired -= ReleaseBullet;
        bullet.bulletExpired += ReleaseBullet;
        bullet.Shoot(direction, damage);
    }

    // 오브젝트 풀에 총알을 반납하는 콜백함수
    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
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



}
