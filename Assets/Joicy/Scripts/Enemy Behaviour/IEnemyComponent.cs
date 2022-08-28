public interface IEnemyComponent
{
    public void Activate();
    public void Deactivate();
}

public interface IEnemyRole : IEnemyComponent
{
}

public interface IEnemyMovement : IEnemyComponent
{
    public delegate void MovementDelegate();
    public MovementDelegate TargetDistanceReached { get; set; }

    public void Move();
}