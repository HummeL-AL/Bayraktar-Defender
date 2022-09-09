using UnityEngine;

public class SpeedBuff : Buff
{
    [SerializeField] protected float acceleration = 0.2f;

    public override void ApplyEffect()
    {
        IEnemyMovement movement = GetComponent<IEnemyMovement>();
        movement.SpeedMultiplier += acceleration;
    }
}
