using UnityEngine;

public class but : Character
{
    protected override void Update()
    {
        if (AttackTargetInRange())
        {
            animator.SetBool("isHeal", true);
        }
        else
        {
            animator.SetBool("isHeal", false);
            Move();
        }
    }
}
