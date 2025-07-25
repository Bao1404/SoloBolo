using UnityEngine;

public class Xe : Character
{
    private AudioSource audioSource;
    public AudioClip attackSound;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayAttackSound();       
        // Kiểm tra nếu Xe có tag "Character" va chạm với kẻ địch (có tag "Enemy")
        if (CompareTag("Character") && collision.gameObject.CompareTag("Enemy"))
        {
            Character enemy = collision.gameObject.GetComponent<Character>();  // Lấy đối tượng Character từ kẻ địch

            Base enemyBase = collision.gameObject.GetComponent<Base>();  // Lấy đối tượng Base từ kẻ địch

            if (enemy != null)
            {
                // Gọi phương thức Die() của kẻ địch để tiêu diệt kẻ địch
                enemy.Die();
                Debug.Log("Enemy destroyed by Character!");
            }

            if (enemyBase != null)
            {
                Destroy(gameObject);
                enemyBase.TakeDamage(200);
            }
        }
        // Kiểm tra nếu kẻ địch có tag "Enemy" va chạm với nhân vật (có tag "Character")
        else if (CompareTag("Enemy") && collision.gameObject.CompareTag("Character"))
        {
            Character character = collision.gameObject.GetComponent<Character>();  // Lấy đối tượng Character từ nhân vật

            Base characterBase = collision.gameObject.GetComponent<Base>();  // Lấy đối tượng Base từ nhân vật

            if (character != null)
            {
                // Gọi phương thức Die() của nhân vật để tiêu diệt nhân vật
                character.Die();
                Debug.Log("Character destroyed by Enemy!");
            }
            if (characterBase != null)
            {
                Destroy(gameObject);
                characterBase.TakeDamage(200);
            }
        }
    }
    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);  // Phát âm thanh một lần
        }
    }

    private void Awake()
    {
        // Tìm AudioSource nếu chưa có
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();  // Nếu không có, tạo một AudioSource mới
        }
    }
}
