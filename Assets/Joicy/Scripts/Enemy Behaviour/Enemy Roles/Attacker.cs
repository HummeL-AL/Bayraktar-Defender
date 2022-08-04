using System.Collections;
using UnityEngine;

public class Attacker : EnemyRole
{
    [SerializeField] IntEventChannel attackChannel = null;

    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int minDamage = 1;
    [SerializeField] private int maxDamage = 3;

    private void Start()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            attackChannel.RaiseEvent(GetDamage());
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private int GetDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }
}
