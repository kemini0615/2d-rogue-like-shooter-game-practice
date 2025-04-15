using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float detectRange;

    [SerializeField] float lerpMultiplier;

    void Start()
    {
        
    }

    void Update()
    {
        // Find a closest enemy
        Collider2D closestEnemy = null;
        Vector2 targetVector = Vector2.up;

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
            transform.up = Vector2.Lerp(transform.up, targetVector, Time.deltaTime * lerpMultiplier);
            return;
        }

        targetVector = (closestEnemy.transform.position - transform.position).normalized;


        transform.up = Vector2.Lerp(transform.up, targetVector, Time.deltaTime * lerpMultiplier);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
