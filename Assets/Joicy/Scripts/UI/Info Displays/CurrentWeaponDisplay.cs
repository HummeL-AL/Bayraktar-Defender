using UnityEngine;
using TMPro;

public class CurrentWeaponDisplay : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _eventChannel = null;
    [SerializeField] private PlayerWeapon _playerWeapon = null;

    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        _eventChannel.ChannelEvent += UpdateValue;
    }

    private void UpdateValue()
    {
        display.text = $"WEP:{_playerWeapon.GetCurrentWeapon().GetName()}";
    }
}
