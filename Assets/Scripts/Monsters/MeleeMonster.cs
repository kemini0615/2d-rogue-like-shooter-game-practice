using System;
using UnityEngine;

public class MeleeMonster : Monster
{
    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRate;
    float attackDelay;
    float attackTimer;

    void Start()
    {
        attackDelay = 1f / attackRate; 
    }

    void Update()
    {
        if (!hasSpawned)
            return;

        if (Player.Instance == null)
            return;

        attackTimer += Time.deltaTime;
    
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        FollowPlayer(distToPlayer);

        if (attackTimer >= attackDelay)
            TryAttack(distToPlayer);
    }

    void FollowPlayer(float distToPlayer)
    {
        if (distToPlayer < 0.01f)
            return;

        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        transform.position = (Vector2) transform.position + direction * moveSpeed * Time.deltaTime;
    }

    void TryAttack(float distToPlayer)
    {
        // 플레이어가 공격 범위 안에 있으면 공격한다
        if (distToPlayer <= attackRange)
            Attack();
    }

    void Attack()
    {
        attackTimer = 0f;

        Player.Instance.TakeDamage(attackDamage);
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
        // 파티클 시스템을 몬스터 게임 오브젝트에서 분리한다
        destroyParticleSystem.transform.parent = null;

        destroyParticleSystem.Play();

        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
