using System;
using UnityEngine;

public class RangeMonster : MonoBehaviour
{
    [SerializeField] SpriteRenderer enemyRenderer;
    [SerializeField] SpriteRenderer spawnIndicatorRenderer;
    [SerializeField] Collider2D colliderComponent;
    [SerializeField] RangeMonsterAttack rangeMonsterAttack;

    [SerializeField] float moveSpeed;

    [SerializeField] float attackRange;

    [SerializeField] bool hasSpawned;

    [SerializeField] int maxHp;
    [SerializeField] int currentHp;

    [SerializeField] ParticleSystem destroyParticleSystem;

    public static Action<int, Vector2> onDamaged;

    void Start()
    {
        // player = FindPlayer();

        SpawnEnemy();

        currentHp = maxHp;
    }

    void Update()
    {
        if (!hasSpawned)
            return;

        if (Player.Instance == null)
            return;
    
        float distToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

        FollowPlayer(distToPlayer);

        rangeMonsterAttack.TryAttack(attackRange, distToPlayer);
    }

    // Player FindPlayer()
    // {
    //     Player player = FindFirstObjectByType<Player>();
    //     if (player == null)
    //     {
    //         Debug.LogWarning("Player not found");
    //         Destroy(gameObject);
    //     }

    //     return player;
    // }

    void SpawnEnemy()
    {
        enemyRenderer.enabled = false;
        spawnIndicatorRenderer.enabled = true;

        // 몬스터 스폰 애니메이션을 재생한다
        Vector3 targetScale = spawnIndicatorRenderer.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicatorRenderer.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(OnSpawnAnimationCompleted);
    }

    // 몬스터 스폰 애니메이션이 끝나면 몬스터를 렌더링한다
    void OnSpawnAnimationCompleted()
    {
        enemyRenderer.enabled = true;
        spawnIndicatorRenderer.enabled = false;

        hasSpawned = true;

        colliderComponent.enabled = true;
    }

    void FollowPlayer(float distToPlayer)
    {
        // 플레이어가 공격 범위 안에 있으면 이동을 멈춘다
        if (distToPlayer <= attackRange)
            return;

        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        transform.position = (Vector2) transform.position + direction * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (currentHp > damage)
        {
            currentHp -= damage;
        }
        else
        {
            currentHp = 0;
            Die();
        }

        onDamaged?.Invoke(damage, transform.position);
    }

    void Die()
    {
        // 파티클 시스템을 몬스터 게임 오브젝트에서 분리한다
        destroyParticleSystem.transform.parent = null;

        destroyParticleSystem.Play();

        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
