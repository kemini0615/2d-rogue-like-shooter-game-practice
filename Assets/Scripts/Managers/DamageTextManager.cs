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

    void InstantiateDamangeText(int damage, Vector2 monsterPosition)
    {
        Vector3 spawnPosition = monsterPosition + Vector2.up * 1.5f;

        DamageText instance = damageTextPool.Get();
        instance.transform.position = spawnPosition;
        instance.FadeOut(damage);

        LeanTween.delayedCall(1f, () => {
            damageTextPool.Release(instance);
        });
    }

    // 오브젝트 풀에 오브젝트가 없을 때, 오브젝트를 생성하는 함수.
    DamageText CreateFunc()
    {
        return Instantiate(prefab, transform);
    }

    // 오브젝트 풀에서 오브젝트를 가져올 때마다(Get() 메소드를 호출할 때마다) 호출되는 액션 함수.
    void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }

    // 오브젝트 풀에 오브젝트를 반납할 때마다(Release() 메소드를 호출할 때마다) 호출되는 액션 함수.
    void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }

    // 오브젝트 풀이 파괴되거나, 더이상 풀에 오브젝트를 저장할 수 없을 때 호출되는 액션 함수.
    void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    void OnDestroy()
    {
        Monster.onDamaged -= InstantiateDamangeText;
    }
}
