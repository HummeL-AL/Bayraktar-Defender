using System.Collections;
using UnityEngine;
using Zenject;

public class Attacker : MonoBehaviour, IEnemyRole
{
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int minDamage = 1;
    [SerializeField] private int maxDamage = 3;

    [Inject] private City city = null;
    private Health cityHealth = null;

    public void Activate()
    {
        enabled = true;
        StartCoroutine(Attack());
    }

    public void Deactivate()
    {
        enabled = false;
    }

    private void Awake()
    {
        cityHealth = city.GetComponent<Health>();
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            cityHealth.TakeDamage(GetDamage());
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private int GetDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }
}
