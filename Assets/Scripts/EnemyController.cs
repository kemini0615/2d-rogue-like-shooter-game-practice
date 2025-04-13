using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ParticleSystem destroyParticleSystem;

    [SerializeField] float moveSpeed;
    [SerializeField] float attackRange;

    void Start()
    {
        // Find and get the player object
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogWarning("Player not found");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        FollowPlayer();
        TryAttack();
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = (Vector2) transform.position + direction * moveSpeed * Time.deltaTime;
    }

    void TryAttack()
    {
        // Check the player is in a range of attack
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // If the player is in the range, then attack him
        if (distToPlayer <= attackRange)
        {
            // temp
            Die();
        }
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
