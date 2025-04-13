using UnityEngine;

// Player class acts like an manager which controlls components in the player game object
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    PlayerHealth playerHealth;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }
}
