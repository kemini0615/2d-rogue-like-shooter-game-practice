using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float detectRange;
    [SerializeField] LayerMask layerMask;

    void Start()
    {
        
    }

    void Update()
    {
        // Find a closest enemy
        Collider2D closestEnemy = null;

        // It is not recommended to call FindObjectsByType() method on every frame
        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectRange, layerMask);

        if (enemies.Length <= 0)
            return;

        float minDistance = detectRange;

        for (int i = 0; i < enemies.Length; i++)
        {
            Collider2D enemy = enemies[i];
            float distance = Vector2.Distance(enemy.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy == null)
        {
            transform.up = Vector2.up;
            return;
        }

        transform.up = (closestEnemy.transform.position - transform.position).normalized;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
