using UnityEngine;

// RangeMonster는 '공격' 기능이 복잡하기 때문에 코드를 분리했다.
// RangeMonsterAttack 클래스가 '공격'을 담당한다.
public class RangeMonster : Monster
{
    [Header("Attack")]
    [SerializeField] RangeMonsterAttack rangeMonsterAttack;

    protected void Update()
    {
        if (!CanFollowPlayer())
            return;

        // 플레이어를 바라보고 추적한다.
        monsterMovement.FacePlayer();
        monsterMovement.FollowPlayer();

        // 공격을 시도한다.
        rangeMonsterAttack.TryAttack();
    }
}
