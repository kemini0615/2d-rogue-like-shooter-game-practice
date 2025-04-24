using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer enemyRenderer;
    [SerializeField] SpriteRenderer spawnIndicatorRenderer;
    [SerializeField] Collider2D collider;


    [SerializeField] float moveSpeed;

    [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;

    [SerializeField] bool hasSpawned;

    [SerializeField] int maxHp;
    [SerializeField] int currentHp;

    [SerializeField] ParticleSystem destroyParticleSystem;

    public static Action<int, Vector2> onDamaged;

    void Start()
    {
        player = FindPlayer();

        SpawnEnemy();

        // Set and calculate attack delay from attack rate
        attackDelay = 1f / attackRate; 

        currentHp = maxHp;
    }

    void Update()
    {
        if (!hasSpawned)
            return;

        if (player == null)
            return;

        attackTimer += Time.deltaTime;
    
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        FollowPlayer(distToPlayer);

        if (attackTimer >= attackDelay)
            TryAttack(distToPlayer);
    }

    Player FindPlayer()
    {
        // Find and get the player object
        Player player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogWarning("Player not found");
            Destroy(gameObject);
        }

        return player;
    }

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

        collider.enabled = true;
    }

    void FollowPlayer(float distToPlayer)
    {
        if (distToPlayer < 0.01f)
            return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = (Vector2) transform.position + direction * moveSpeed * Time.deltaTime;
    }

    void TryAttack(float distToPlayer)
    {
        // 플레이어가 공격 범위 안에 있으면 공격한다
        if (distToPlayer <= attackRange)
            Attack();
    }

    void Attack()
    {
        attackTimer = 0f;

        player.TakeDamage(attackDamage);
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
