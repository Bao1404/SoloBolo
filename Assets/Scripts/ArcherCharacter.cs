using System.Collections;
using UnityEngine;

public class ArcherCharacter : Character
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float arrowFlightTime = 0.6f; // thời gian mũi tên bay tới đích

    protected override void Attack()
    {
        animator.SetBool("isAttack", true); // Animation sẽ gọi ShootArrow()
    }

    // Gọi từ Animation Event tại frame bắn
    public void ShootArrow()
    {
        if (enemy == null) return;

        GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrow = arrowObj.GetComponent<Arrow>();

        float timeToTarget = 0.2f; // 👈 giảm để tăng tốc độ bay
        arrow.Launch(enemy.transform.position, timeToTarget);
    }
}