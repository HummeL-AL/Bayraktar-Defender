using System.Collections;
using UnityEngine;
using Zenject;

public class Attacker : MonoBehaviour, IActivableRole
{
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int minDamage = 1;
    [SerializeField] private int maxDamage = 3;

    [Inject] private City city = null;

    private IEnumerator attack = null;

    public void Activate()
    {
        enabled = true;
        StartCoroutine(attack);
    }

    public void Deactivate()
    {
        enabled = false;
        StopCoroutine(attack);
    }

    public void SetDefault()
    {
    }

    private void Awake()
    {
        attack = Attack();
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            city.TakeDamage(GetDamage(), 0);
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private int GetDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }
}
