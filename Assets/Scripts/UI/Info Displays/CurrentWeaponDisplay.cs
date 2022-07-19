using UnityEngine;
using TMPro;

public class CurrentWeaponDisplay : MonoBehaviour
{
    [SerializeField] private PlayerWeapon _playerWeapon = null;

    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        GameEventHandler.WeaponSwitched += UpdateValue;
    }

    private void UpdateValue()
    {
        display.text = $"WEP:{_playerWeapon.GetCurrentWeapon().GetName()}";
    }
}
