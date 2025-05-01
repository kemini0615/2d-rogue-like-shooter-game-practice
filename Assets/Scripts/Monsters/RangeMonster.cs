using UnityEngine;

public class RangeMonster : Monster
{
    [Header("Attack")]
    [SerializeField] RangeMonsterAttack rangeMonsterAttack;

    protected void Update()
    {
        if (!CanFollowPlayer())
            return;

        monsterMovement.FacePlayer();
        monsterMovement.FollowPlayer();

        rangeMonsterAttack.TryAttack();
    }
}
