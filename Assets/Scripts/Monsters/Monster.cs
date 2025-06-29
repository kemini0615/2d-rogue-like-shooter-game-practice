using System;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected MonsterMovement monsterMovement;
    [SerializeField] protected SpriteRenderer monsterRenderer;
    [SerializeField] protected SpriteRenderer spawnIndicatorRenderer;
    [SerializeField] protected CircleCollider2D colliderComponent;
    [SerializeField] protected ParticleSystem destroyParticleSystem;

    [Header("Health")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected int currentHp;

    [Header("Flags")]
    [SerializeField] protected bool hasSpawned;
    
    [Header("Events")]
    public static Action<int, Vector2> onDamaged;

    protected virtual void Start()
    {
        currentHp = maxHp;
        Spawn();
    }

    protected void Spawn()
    {
        monsterRenderer.enabled = false;
        spawnIndicatorRenderer.enabled = true;

        // 몬스터 스폰 애니메이션을 재생한다.
        Vector3 targetScale = spawnIndicatorRenderer.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicatorRenderer.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(OnSpawnAnimationCompleted);
    }

    // 몬스터 스폰 애니메이션이 끝나면 몬스터를 렌더링한다.
    protected void OnSpawnAnimationCompleted()
    {
        monsterRenderer.enabled = true;
        spawnIndicatorRenderer.enabled = false;
        colliderComponent.enabled = true;

        hasSpawned = true;
    }

    protected virtual bool CanFollowPlayer()
    {
        if (!hasSpawned || Player.Instance == null)
            return false;

        return true;
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

    protected void Die()
    {
        // 파티클 시스템을 몬스터 게임 오브젝트에서 분리한다.
        destroyParticleSystem.transform.parent = null;
        destroyParticleSystem.Play();
        Destroy(gameObject);
    }

    public Vector2 GetCenterPosition()
    {
        return (Vector2) transform.position + new Vector2(0, colliderComponent.radius);
    }
}
