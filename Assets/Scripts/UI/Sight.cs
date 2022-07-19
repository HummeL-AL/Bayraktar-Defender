using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [SerializeField] private PlayerWeapon _playerWeapon = null;
    [SerializeField] private Vector2 _minMaxSize = Vector2.one;

    private RectTransform _rectTransform = null;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        GameEventHandler.WeaponStatsChanged += UpdateSight;
    }

    private void UpdateSight()
    {
        float heat = _playerWeapon.GetCurrentWeapon().GetHeat();
        float targetSize = Mathf.Lerp(_minMaxSize.x, _minMaxSize.y, heat);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetSize);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetSize);
    }
}
