using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField] DamageText prefab;

    ObjectPool<DamageText> damageTextPool;

    void Awake()
    {
        Monster.onDamaged += InstantiateDamangeText;
    }

    void Start()
    {
           damageTextPool = new ObjectPool<DamageText>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);;
    }

    // 오브젝트 풀에서 더이상 가져올 오브젝트가 없을 때 오브젝트를 생성하기 위해서 실행되는 함수
    DamageText CreateFunc()
    {
        return Instantiate(prefab, transform);
    }

    // 오브젝트 풀에서 오브젝트를 가져올 때 실행되는 함수
    void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }

    // 오브젝트 풀에 오브젝트를 반납할 때 실행되는 함수
    void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }

    // 오브젝트 풀의 오브젝트가 최대 개수에 도달해서 오브젝트를 파괴하기로 결정했을 때 실행되는 함수
    void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    void InstantiateDamangeText(int damage, Vector2 enemyPosition)
    {
        Vector3 spawnPosition = enemyPosition + Vector2.up * 1.5f;
        DamageText instance = damageTextPool.Get(); // 오브젝트 풀에서 오브젝트를 가져온다
        instance.transform.position = spawnPosition;
        instance.FadeOut(damage);

        LeanTween.delayedCall(1f, () => {
            damageTextPool.Release(instance); // 오브젝트 풀에 오브젝트를 반납한다
        });
    }

    void OnDestroy()
    {
        Monster.onDamaged -= InstantiateDamangeText;
    }
}
