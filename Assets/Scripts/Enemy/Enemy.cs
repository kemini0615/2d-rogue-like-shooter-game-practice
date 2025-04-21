using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer enemyRenderer;
    [SerializeField] SpriteRenderer spawnIndicatorRenderer;


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
        Player player = FindPlayer();

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
        // Hide enemy and show spawn indicator
        enemyRenderer.enabled = false;
        spawnIndicatorRenderer.enabled = true;

        // Play animation of spawn indicator and then, show enemy and hide spawn indicator
        Vector3 targetScale = spawnIndicatorRenderer.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicatorRenderer.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(OnSpawnAnimationCompleted);
    }

    void OnSpawnAnimationCompleted()
    {
        enemyRenderer.enabled = true;
        spawnIndicatorRenderer.enabled = false;

        hasSpawned = true;
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
        // If the player is in the range, then attack him
        if (distToPlayer <= attackRange)
            Attack();
    }

    void Attack()
    {
        // If you attack successfully, then reset attack timer 
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
        // Unparent particle system from game object
        destroyParticleSystem.transform.parent = null;

        // Destroy game object
        Destroy(gameObject);

        // Play particle system
        destroyParticleSystem.Play();

        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
