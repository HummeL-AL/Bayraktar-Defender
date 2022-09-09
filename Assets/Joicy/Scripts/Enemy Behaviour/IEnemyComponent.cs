public interface IEnemyComponent
{
    public void Activate();
    public void Deactivate();
    public void SetDefault();
}

public interface IEnemyRole : IEnemyComponent
{
}

public interface IPermanentRole : IEnemyRole
{
}

public interface IActivableRole : IEnemyRole
{
}

public interface IEnemyMovement : IEnemyComponent
{
    public delegate void MovementDelegate();
    public MovementDelegate TargetDistanceReached { get; set; }

    public float SpeedMultiplier { get; set; }

    public void Move();
}