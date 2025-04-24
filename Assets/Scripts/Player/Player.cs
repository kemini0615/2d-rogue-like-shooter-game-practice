using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;

    [SerializeField] int maxHp;
    [SerializeField] int currentHp;

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
}
