using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    [SerializeField] private Image HPBar;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private Collider2D hitboxCollider; // Kích thước của hitbox collider
    private float currentHp;

    private void Start()
    {
        currentHp = maxHp;
        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            DestroyBase();
        }
    }

    public void RegenHP(float regenAmount)
    {
        currentHp += regenAmount;
        currentHp = Mathf.Min(currentHp, maxHp);  // Đảm bảo máu không vượt quá maxHp
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (HPBar != null)
        {
            HPBar.fillAmount = currentHp / maxHp;
        }
    }

    public void DestroyBase()
    {
        Destroy(gameObject);
        if (CompareTag("Character"))
        {
            GameManager.instance.GameOver();
        }
        else if (CompareTag("Enemy"))
        {
            GameManager.instance.Win();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm có tag phù hợp
        if (CompareTag("Character") && collision.gameObject.CompareTag("Enemy"))
        {
            // Nhận sát thương từ kẻ địch có tag "Enemy"
            Character enemy = collision.gameObject.GetComponent<Character>();
            if (enemy != null)
            {
                TakeDamage(enemy.attackDamage);  // Giả sử Character có thuộc tính attackDamage
            }
        }
        else if (CompareTag("Enemy") && collision.gameObject.CompareTag("Character"))
        {
            // Nhận sát thương từ nhân vật có tag "Character"
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                TakeDamage(character.attackDamage);  // Giả sử Character có thuộc tính attackDamage
            }
        }
    }
}
