using System.Collections;
using UnityEngine;

public class Attacker : EnemyRole
{
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _minDamage = 1;
    [SerializeField] private int _maxDamage = 3;

    public delegate void AttackDelegate(int damage);
    public event AttackDelegate Attacking = null;

    private void Start()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            Attacking.Invoke(GetDamage());
            yield return new WaitForSeconds(_attackCooldown);
        }
    }

    private int GetDamage()
    {
        return Random.Range(_minDamage, _maxDamage + 1);
    }
}
