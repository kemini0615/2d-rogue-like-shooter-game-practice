using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    enum State
    {
        Idle,
        Attack,
    }

    State state;

    [SerializeField] Transform hitSpotTransform;
    [SerializeField] float hitSpotRadius;

    [SerializeField] LayerMask layerMask;
    [SerializeField] float detectRange;
    [SerializeField] int damage;

    [SerializeField] float lerpMultiplier;

    [SerializeField] Animator animator;

    List<Enemy> attackedEnemies = new List<Enemy>();

    void Start()
    {
        state = State.Idle;
    }

    void Update()
    {
        // temp
        if (Input.GetMouseButton(0))
        {
            StartAttack();
        }

        // State machine pattern
        switch (state)
        {
            case State.Idle:
                AimAtClosestEnemy();
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }   
    }

    void StartAttack()
    {
        attackedEnemies.Clear();
        animator.Play("Attack");
        state = State.Attack;
    }

    void UpdateAttack()
    {
        Attack();
    }
    
    void ExitAttack()
    {
        state = State.Idle;
        attackedEnemies.Clear();
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitSpotTransform.position, hitSpotRadius, layerMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy targetEnemy = enemies[i].GetComponent<Enemy>();
            if (!attackedEnemies.Contains(targetEnemy))
            {
                targetEnemy.TakeDamage(damage);
                attackedEnemies.Add(targetEnemy);
            }
        }
    }

    private void AimAtClosestEnemy()
    {
        Enemy closestEnemy = FindClosestEnemy();

        Vector2 targetVector = Vector2.up;

        if (closestEnemy != null)
        {
            targetVector = (closestEnemy.transform.position - transform.position).normalized;
        }

        transform.up = Vector2.Lerp(transform.up, targetVector, Time.deltaTime * lerpMultiplier);
    }

    private Enemy FindClosestEnemy()
    {
        Enemy closestEnemy = null;

        // It is not recommended to call FindObjectsByType() method on every frame
        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectRange, layerMask);

        if (enemies.Length <= 0)
            return null;

        float minDistance = detectRange;

        for (int i = 0; i < enemies.Length; i++)
        {
            Collider2D enemy = enemies[i];
            float distance = Vector2.Distance(enemy.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy.GetComponent<Enemy>();
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireSphere(hitSpotTransform.position, hitSpotRadius);
    }
}
