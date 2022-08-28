using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _armorLevel = 0;
    [SerializeField] private int _healthPoints = 100;

    public delegate void DamageDelegate(int damage);
    public DamageDelegate DamageTaken = null;

    public delegate void DeathDelegate();
    public DeathDelegate Died = null;

    public int GetArmor()
    {
        return _armorLevel;
    }

    public void TakeDamage(int damage)
    {
        if (_healthPoints > 0)
        {
            _healthPoints -= damage;
            DamageTaken?.Invoke(damage);

            if (_healthPoints <= 0)
            {
                Died?.Invoke();
            }
        }
    }
}
