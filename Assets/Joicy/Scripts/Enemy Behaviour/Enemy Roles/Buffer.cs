using UnityEngine;
using Zenject;

public class Buffer : MonoBehaviour, IPermanentRole
{
    [SerializeField] protected string buffType = null;
    [SerializeField] protected bool localBuffer = true;
    [SerializeField] protected float buffDistance = 1f;

    [Inject] private LevelStats levelStats = null;

    public void Activate()
    {
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }

    public void SetDefault()
    {
    }

    private void Update()
    {
        if(localBuffer)
        {
            BuffLocal();
        }
        else
        {
            BuffGlobal();
        }
    }

    private void BuffGlobal()
    {
        foreach(Enemy enemy in levelStats.Enemies)
        {
            enemy.AddBuff(buffType);
        }
    }

    private void BuffLocal()
    {
        Vector3 position = transform.position;

        foreach (Collider collider in Physics.OverlapSphere(position, buffDistance, 1 << 3))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if(enemy)
            {
                enemy.AddBuff(buffType);
            }
        }
    }
}
