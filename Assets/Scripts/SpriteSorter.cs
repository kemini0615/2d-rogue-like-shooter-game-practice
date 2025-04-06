using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = (int) (transform.position.y * -10);
    }
}
