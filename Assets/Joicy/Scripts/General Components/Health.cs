using UnityEngine;

public class Health : MonoBehaviour, IModelSwitcher, IDamageable
{
    public delegate void DamageDelegate(int damage);
    public DamageDelegate DamageTaken = null;

    public delegate void DeathDelegate();
    public DeathDelegate Died = null;

    public LODGroup[] Models { get => models; set => models = value; }
    public LODSwitcher Switcher { get => switcher; set => switcher = value; }

    [SerializeField] protected int _armorLevel = 0;
    [SerializeField] protected int _healthPoints = 100;
    [SerializeField] protected int _maxHealthPoints = 100;

    [SerializeField] private LODSwitcher switcher = null;
    [SerializeField] private LODGroup[] models = null;
    [SerializeField] private bool staging = false;

    private int stage = 0;
    private float stageStep = 1;

    public int HealthPoints { get => _healthPoints; private set => _healthPoints = Mathf.Clamp(value, 0, _maxHealthPoints); }

    public void TakeDamage(int damage, int damageLevel)
    {
        if (HealthPoints > 0 && damageLevel >= _armorLevel)
        {
            HealthPoints -= damage;
            DamageTaken?.Invoke(damage);

            if (HealthPoints <= 0)
            {
                Died?.Invoke();
            }
        }
    }

    public void TakeDamage(int damage, int damageLevel, float damageRadius)
    {
        TakeDamage(damage, damageLevel);
    }

    private void Awake()
    {
        DamageTaken += OnDamageTaken;
        Died += OnDeath;

        _healthPoints = _maxHealthPoints;
        stageStep = 1f / (models.Length - 1);

        Switcher = GetComponent<LODSwitcher>();
    }

    private void UpdateStage()
    {
        float healthPercent = 1f - (float)HealthPoints / _maxHealthPoints;
        int newStage = Mathf.FloorToInt(healthPercent / stageStep);

        if(newStage != stage)
        {
            stage = newStage;
            SetModelID(stage);
        }
    }

    private void SetRandomModel()
    {
        IModelSwitcher modelSwitcher = this;
        modelSwitcher.SetModel();
    }

    private void SetModelID(int index)
    {
        IModelSwitcher modelSwitcher = this;
        modelSwitcher.SetModel(index);
    }

    private void OnDamageTaken(int damage)
    {
        if (staging)
        {
            UpdateStage();
        }
    }

    private void OnDeath()
    {
        if (!staging)
        {
            SetRandomModel();
        }

        foreach(Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
    }
}
