using UnityEngine;

public class ArcherEnemy : Enemy
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
        if (character == null) return;

        GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrow = arrowObj.GetComponent<Arrow>();

        float timeToTarget = 0.2f;
        arrow.Launch(character.transform.position, 0.3f, "Character");
    }
}