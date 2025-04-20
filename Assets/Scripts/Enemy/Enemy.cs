using System;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHp;
    [SerializeField] int currentHp;

    [SerializeField] ParticleSystem destroyParticleSystem;

    public static Action<int, Vector2> onDamaged;

    void Start()
    {
        currentHp = maxHp;
    }

    void Update()
    {
        
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
}
