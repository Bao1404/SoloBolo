using System.Collections;
using UnityEngine;

public class ArcherCharacter : Character
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float arrowFlightTime = 0.6f;

    protected override void Attack()
    {
        animator.SetBool("isAttack", true);
    }
    public void ShootArrow()
    {
        if (enemy == null) return;

        GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrow = arrowObj.GetComponent<Arrow>();

        float timeToTarget = 0.2f;
        arrow.Launch(enemy.transform.position, 0.3f, "Enemy");
    }
}