using UnityEngine;

[System.Serializable]
public struct ShootingStats
{
    [SerializeField] private Vector2 _spreadAngle;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private bool _overheatable;
    [SerializeField] private float _shotDelay;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _shotHeat;
    [SerializeField] private float _coolingSpeed;
    [SerializeField] private AudioClip _shootSound;

    public Vector2 SpreadAngle { get => _spreadAngle; }
    public int MaxAmmo { get => _maxAmmo; }
    public bool Overheatable { get => _overheatable; }
    public float ShotDelay { get => _shotDelay; }
    public float ReloadTime { get => _reloadTime; }
    public float ShotHeat { get => _shotHeat; }
    public float CoolingSpeed { get => _coolingSpeed; }
    public AudioClip ShootSound { get => _shootSound; }
}
