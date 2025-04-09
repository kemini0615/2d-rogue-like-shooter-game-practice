using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float moveSpeed;

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
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = (Vector2) transform.position + direction * moveSpeed * Time.deltaTime;
    }
}
