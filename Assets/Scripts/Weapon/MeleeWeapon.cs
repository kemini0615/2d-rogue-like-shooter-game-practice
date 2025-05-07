using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Components")]
    [SerializeField] Transform hitSpotTransform;
    [SerializeField] BoxCollider2D hitSpotCollider;

    protected override void Start()
    {
        base.Start();

        hitSpotCollider = hitSpotTransform.GetComponent<BoxCollider2D>();
    }

    // 조건이 충족되면 공격한다
    protected override void TryAttack()
    {
        if (attackTimer < attackDelay)
            return;

        attackTimer = 0;
        MeleeAttack();
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
