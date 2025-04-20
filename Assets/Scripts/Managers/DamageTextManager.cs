using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField] DamageText prefab;

    void Awake()
    {
        Enemy.onDamaged += InstantiateDamangeText;
    }

    void OnDestroy()
    {
        Enemy.onDamaged -= InstantiateDamangeText;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void InstantiateDamangeText(int damage, Vector2 enemyPosition)
    {
        Vector3 spawnPosition = enemyPosition + Vector2.up * 1.5f;
        DamageText instance = Instantiate(prefab, spawnPosition, Quaternion.identity);
        instance.FadeOut(damage);
    }
}
