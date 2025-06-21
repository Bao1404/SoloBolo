using UnityEngine;

public class ShieldCharater : Character
{
    [SerializeField] private float shieldBlockDame = 5f;
    protected override void Update()
    {
        //if(DetectEnemyInRange() && AttackEnemyInRange())
        //{
        //    Attack();
        //}
        //else
        //{
        //    Move();
        //}
    }
    protected override void Attack()
    {
        animator.SetBool("isHoldingShield", true);
    }
    //public override void TakeDame(float damage)
    //{
    //    if (DetectEnemyInRange() && AttackEnemyInRange())
    //    {
    //        base.TakeDame(damage - shieldBlockDame);
    //    }
    //    else
    //    {
    //        base.TakeDame(damage);
    //    }
    //}
}
