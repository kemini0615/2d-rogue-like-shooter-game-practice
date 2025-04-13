using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int hp;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (hp > damage)
        {
            hp -= damage;
        }
        else
        {
            hp = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Plyer died");
        Destroy(gameObject);
    }
}
