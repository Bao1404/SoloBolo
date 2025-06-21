using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ArcherCharacter : Character
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float arrowFlightTime = 0.6f;  // Time to reach the target
    private GameObject enemy;  // Reference to the enemy character

    // You may want to find the closest target automatically
    protected override void Start()
    {
        base.Start();
        // Optionally, find the closest enemy character to shoot at
        enemy = FindClosestTarget();
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
