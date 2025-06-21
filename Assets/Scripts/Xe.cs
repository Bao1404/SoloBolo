using UnityEngine;

public class Xe : Character
{
    protected override void Update()
    {
        if (AttackTargetInRange())
        {
            Move();
            Attack();
        }
        else
        {
            Move();
        }
    }
    protected override void Attack()
    {
        base.Attack();
    }
}
