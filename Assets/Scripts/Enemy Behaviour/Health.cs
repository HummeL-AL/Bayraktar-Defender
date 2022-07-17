using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _healthPoints = 100;

    public delegate void DeathDelegate();
    public DeathDelegate Died = null;

    public void TakeDamage(int damage)
    {
        _healthPoints -= damage;
        if (_healthPoints < 0)
        {
            Died.Invoke();
        }
    }
}
