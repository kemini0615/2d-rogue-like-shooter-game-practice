using System;
using System.Collections;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D bulletCollider;

    [SerializeField] float speed;
    int attackDamage;

    public Action<MonsterBullet> expired;
    float lifetime = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();
    }

    // 총알의 초기 위치와 속도를 설정한다.
    public void Init(Vector2 initialPosition)
    {
        transform.position = initialPosition;
        rb.linearVelocity = Vector2.zero;
        this.bulletCollider.enabled = true;

        StartCoroutine(ReleaseCoroutine());
    }

    public void Shoot(Vector2 direction, int attackDamage)
    {
        transform.right = direction;
        rb.linearVelocity = direction * speed; // Rigidbody.velocity는 더이상 사용하지 않는다.
        this.attackDamage = attackDamage;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.TryGetComponent(out Player player))
        {
            StopCoroutine(ReleaseCoroutine());

            player.TakeDamage(this.attackDamage);

            this.bulletCollider.enabled = false;
            
            // 플레이어와 충돌하면 오브젝트 풀에 오브젝트(총알)를 반납한다.
            expired?.Invoke(this);
        }
    }

    IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(lifetime); // 총알의 생명 주기만큼 대기한다.

        // 플레이어와 충돌하지 않은 채로 생명 주기만큼의 시간이 지나면 오브젝트 풀에 오브젝트(총알)를 반납한다.
        expired?.Invoke(this);
    }
}
