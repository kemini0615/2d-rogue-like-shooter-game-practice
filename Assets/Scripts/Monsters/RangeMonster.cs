using UnityEngine;

public class RangeMonster : Monster
{
    [Header("Attack")]
    [SerializeField] RangeMonsterAttack rangeMonsterAttack;

    protected void Update()
    {
        if (!CanFollowPlayer())
            return;

        FollowPlayer();

        rangeMonsterAttack.TryAttack();
    }
}
