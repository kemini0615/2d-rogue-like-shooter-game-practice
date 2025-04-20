using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TextMeshPro damageText;

    public void FadeOut(int damage)
    {
        animator.Play("FadeOut");
        damageText.text = damage.ToString();
    }
}
