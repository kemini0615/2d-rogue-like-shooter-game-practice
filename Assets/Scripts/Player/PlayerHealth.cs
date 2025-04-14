using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
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

    void Update()
    {
        
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
