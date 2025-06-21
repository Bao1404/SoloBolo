using UnityEngine;

public class Xe : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Update()
    {
        if (AttackTargetInRange())
        {
            Attack();
        }
        else
        {
            Move();
        }
    }
}
