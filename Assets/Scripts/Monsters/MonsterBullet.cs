using System;
using System.Collections;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D bulletCollider;

    [SerializeField] float speed;
    int attackDamage;

    public Action<MonsterBullet> bulletExpired;
    float life = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();
    }

    public void Init(Vector2 initialPosition)
    {
        transform.position = initialPosition;
        rb.linearVelocity = Vector2.zero;
        this.bulletCollider.enabled = true;
        
        // 코루틴 시작
        StartCoroutine(ReleaseCoroutine());
    }

    public void Shoot(Vector2 direction, int attackDamage)
    {
        transform.right = direction;
        // Rigidbody.velocity는 더이상 사용하지 않는다
        rb.linearVelocity = direction * speed;
        this.attackDamage = attackDamage;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // 충돌한 상대가 플레이어라면 공격
        if (otherCollider.TryGetComponent(out Player player))
        {
            // 코루틴 종료
            StopCoroutine(ReleaseCoroutine());

            player.TakeDamage(this.attackDamage);

            this.bulletCollider.enabled = false;
            
            // 플레이어와 충돌하면 오브젝트 풀에 반납
            bulletExpired?.Invoke(this);
        }
    }

    // 코루틴 함수
    IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(life); // N초 대기

        // 플레이어와 충돌하지 않은 채로 일정 시간(N초)이 지나면 오브젝트 풀에 반납
        bulletExpired?.Invoke(this);
    }
}
