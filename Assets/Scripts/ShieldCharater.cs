using UnityEngine;

public class ShieldCharater : Character
{
    [SerializeField] private float shieldBlockDame = 5f;
    //protected override void Update()
    //{
    //    if (AttackTargetInRange())
    //    {
    //        animator.SetBool("isHoldingShield", true);
    //        Attack();
    //    }
    //    else
    //    {
    //        animator.SetBool("isHoldingShield", false);
    //        Move();
    //    }
    //}
    //public override void TakeDamage(float damage)
    //{
    //    if (AttackTargetInRange())
    //    {
    //        currentHp -= (damage - shieldBlockDame);
    //        currentHp = Mathf.Max(currentHp, 0);  // Ensure health doesn't go below 0
    //        UpdateHealthBar();
    //    }
    //    else
    //    {
    //        currentHp -= damage;
    //        currentHp = Mathf.Max(currentHp, 0);  // Ensure health doesn't go below 0
    //        UpdateHealthBar();
    //    }

    //    if (currentHp <= 0)
    //    {
    //        Die();  // If health is 0 or less, call Die function
    //    }
    //}
}
