using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHp;
    [SerializeField] int currentHp;

    [SerializeField] ParticleSystem destroyParticleSystem;

    [SerializeField] TextMeshPro hpText;

    void Start()
    {
        currentHp = maxHp;
        hpText.text = currentHp.ToString();
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

        hpText.text = currentHp.ToString();
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
