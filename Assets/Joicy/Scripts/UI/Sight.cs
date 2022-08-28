using UnityEngine;

public class Sight : MonoBehaviour
{
    [SerializeField] private VoidEventChannel weaponStatsChangedChannel = null;

    [SerializeField] private PlayerWeapon playerWeapon = null;
    [SerializeField] private Vector2 minMaxSize = Vector2.one;

    private RectTransform _rectTransform = null;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        weaponStatsChangedChannel.ChannelEvent += UpdateSight;
    }

    private void UpdateSight()
    {
        float heat = playerWeapon.CurrentWeapon.Heat;
        float targetSize = Mathf.Lerp(minMaxSize.x, minMaxSize.y, heat);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetSize);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetSize);
    }

    private void OnDestroy()
    {
        weaponStatsChangedChannel.ChannelEvent -= UpdateSight;
    }
}
