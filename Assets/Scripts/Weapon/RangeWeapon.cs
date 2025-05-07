using UnityEngine;

public class RangeWeapon : Weapon
{
    [Header("Components")]
    Transform shootingPoint;

    // 조건이 충족되면 공격한다
    protected override void TryAttack()
    {
        // 조건 1: 공격 타이머
        if (attackTimer < attackDelay)
            return;

        // 조건 2: 공격 사정거리
        // TODO

        attackTimer = 0;
        RangeAttack();
    }

    protected void RangeAttack()
    {
        // TODO
    }
}
