using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Weapon_", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private bool unlockedByDefault = false;
    [SerializeField] private WeaponStats[] _weaponLevels = null;

    public bool UnlockedByDefault { get => unlockedByDefault; }
    public int MaxLevel { get => _weaponLevels.Length - 1; }

    public WeaponStats GetWeaponStats(int level)
    {
        return _weaponLevels[level];
    }
}
