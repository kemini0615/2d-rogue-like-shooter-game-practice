using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer enemyRenderer;
    [SerializeField] SpriteRenderer spawnIndicatorRenderer;
    [SerializeField] ParticleSystem destroyParticleSystem;

    [SerializeField] float moveSpeed;
    [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;
    [SerializeField] bool hasSpawned;

    void Start()
    {
        // Find and get the player object
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogWarning("Player not found");
            Destroy(gameObject);
        }

        // Spawn enemy
        // 1. Hide enemy and show spawn indicator
        enemyRenderer.enabled = false;
        spawnIndicatorRenderer.enabled = true;

        // 2. Play animation of spawn indicator
        // 3. Show enemy and hide spawn indicator
        Vector3 targetScale = spawnIndicatorRenderer.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicatorRenderer.gameObject, targetScale, .3f).setLoopPingPong(4).setOnComplete(OnSpawnAnimationCompleted);

        // Set and calculate attack delay from attack rate
        attackDelay = 1f / attackRate; 
    }

    void Update()
    {
        if (!hasSpawned)
            return;

        FollowPlayer();

        if (attackTimer >= attackDelay)
            TryAttack();
    }

    void OnSpawnAnimationCompleted()
    {
        enemyRenderer.enabled = true;
        spawnIndicatorRenderer.enabled = false;
        hasSpawned = true;
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = (Vector2) transform.position + direction * moveSpeed * Time.deltaTime;

        attackTimer += Time.deltaTime;
    }

    void TryAttack()
    {
        // Check the player is in a range of attack
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // If the player is in the range, then attack him
        if (distToPlayer <= attackRange)
            Attack();
    }

    void Attack()
    {
        // If you attack successfully, then reset attack timer 
        attackTimer = 0f;

        Debug.Log($"Attacking player by {attackDamage} damage");
    }

    void Die()
    {
        // Unparent particle system from game object
        destroyParticleSystem.transform.parent = null;

        // Destroy game object
        Destroy(gameObject);

        // Play particle system
        destroyParticleSystem.Play();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
