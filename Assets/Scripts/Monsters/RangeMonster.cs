using UnityEngine;

public class RangeMonster : Monster
{
    [Header("Attack")]
    [SerializeField] RangeMonsterAttack rangeMonsterAttack;

    protected void Update()
    {
        if (!CanFollowPlayer())
            return;

        FacePlayer();
        FollowPlayer();

        rangeMonsterAttack.TryAttack();
    }

    void FacePlayer()
    {
        bool facingRight = Player.Instance.transform.position.x > transform.position.x;
        transform.localScale = facingRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }
}
