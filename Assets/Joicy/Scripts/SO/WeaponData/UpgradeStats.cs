using UnityEngine;

[System.Serializable]
public struct UpgradeStats
{
    [SerializeField] private int _price;

    public int Price { get => _price; }
}
