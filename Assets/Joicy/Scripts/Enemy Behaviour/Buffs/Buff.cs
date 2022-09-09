using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    private float duration = 0;
    private int stack = 0;

    [SerializeField] protected float timeIncrease = 2;
    [SerializeField] protected float maxTime = 10;
    [SerializeField] protected int maxStack = 1;

    protected float Duration { get => duration; set => duration = Mathf.Clamp(value, 0, maxTime); }
    protected float Stack { get => stack; set => stack = (int)Mathf.Clamp(value, 0, maxStack); }

    public virtual void ApplyEffect()
    {
    }

    public void Activate()
    {
        Duration = timeIncrease;
    }

    public void Renovate()
    {
        Duration += timeIncrease;
        Stack++;
    }

    protected void Update()
    {
        MakeTick();
    }

    private void MakeTick()
    {
        Duration -= Time.deltaTime;
        if(Duration <= 0)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        Destroy(this);
    }
}
