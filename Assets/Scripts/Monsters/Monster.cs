using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected SpriteRenderer monsterRenderer;
    [SerializeField] protected SpriteRenderer spawnIndicatorRenderer;
    [SerializeField] protected Collider2D colliderComponent;
    [SerializeField] protected ParticleSystem destroyParticleSystem;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed;

    [Header("Health")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected int currentHp;

    [Header("Flags")]
    [SerializeField] protected bool hasSpawned;
    
    [Header("Events")]
    public static Action<int, Vector2> onDamaged;

    protected void Start()
    {
        currentHp = maxHp;
        Spawn();
    }

    protected void Spawn()
    {
        monsterRenderer.enabled = false;
        spawnIndicatorRenderer.enabled = true;

        // 몬스터 스폰 애니메이션을 재생한다
        Vector3 targetScale = spawnIndicatorRenderer.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicatorRenderer.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(OnSpawnAnimationCompleted);
    }

    // 몬스터 스폰 애니메이션이 끝나면 몬스터를 렌더링한다
    protected void OnSpawnAnimationCompleted()
    {
        monsterRenderer.enabled = true;
        spawnIndicatorRenderer.enabled = false;
        colliderComponent.enabled = true;

        hasSpawned = true;
    }

    protected void FollowPlayer(float distToPlayer)
    {
        if (distToPlayer < 0.01f)
            return;

        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        transform.position = (Vector2) transform.position + direction * moveSpeed * Time.deltaTime;
    }
}
