using UnityEngine;

[System.Serializable]
public struct ControllableStats
{
    [SerializeField] private Vector2 _sensivity;

    public Vector2 Sensivity { get => _sensivity; }
}
