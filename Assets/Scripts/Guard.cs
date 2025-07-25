using System.Collections;
using UnityEngine;

public class GuardCharacter : Character
{
    private AudioSource audioSource;  // Thêm AudioSource vào Guard
    public AudioClip attackSound;     // Thêm AudioClip cho âm thanh tấn công
    
    private bool isAttacking = false;
    private bool canAttack = true;    // Biến kiểm tra xem có thể tấn công hay không

    protected override void Attack()
    {
        if (attackCollider == null || !canAttack) return;  // Nếu đang tấn công hoặc không thể tấn công, dừng lại

        isAttacking = true;
        SetAttackAnimation(true);
        PlayAttackSound();  // Gọi hàm để phát âm thanh khi tấn công
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;  // Đặt canAttack thành false để ngừng tấn công cho đến khi đủ thời gian trễ

        attackCollider.enabled = true;   // Bật collider khi tấn công
        yield return new WaitForSeconds(1f);  // Chờ 1 giây (thời gian tấn công)

        attackCollider.enabled = false;  // Tắt collider sau khi kết thúc tấn công
        SetAttackAnimation(false);
        isAttacking = false;

        yield return new WaitForSeconds(1f);  // Đợi thêm 1 giây giữa các đòn tấn công

        canAttack = true;  // Cho phép tấn công tiếp khi thời gian trễ đã hết
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
