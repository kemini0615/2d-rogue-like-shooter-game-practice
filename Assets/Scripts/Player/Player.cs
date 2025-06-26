using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // 싱글톤 패턴.
    private static Player instance;
    public static Player Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] CircleCollider2D playerCollider;

    [SerializeField] int maxHp;
    [SerializeField] int currentHp;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        currentHp = maxHp;
        UpdateHpBar();
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

        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        hpSlider.value = (float) currentHp / maxHp;
        hpText.text = currentHp + " / " + maxHp;
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    public Vector2 GetCenterPosition()
    {
        return (Vector2) transform.position + playerCollider.offset;
    }
}
