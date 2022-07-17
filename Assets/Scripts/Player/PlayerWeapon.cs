using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon[] _weapons = null;
    [SerializeField] private Weapon _choosedWeapon = null;

    public Weapon GetCurrentWeapon()
    {
        return _choosedWeapon;
    }

    public Weapon GetWeapon(int id)
    {
        return _weapons[id];
    }

    public int GetWeaponCount()
    {
        return _weapons.Length;
    }

    public void OnLeftMouseButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            StartCoroutine("Shooting");
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            StopCoroutine("Shooting");
        }
    }

    public void OnNextWeaponChoosing(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ChooseNextWeapon();
        }
    }

    public void OnPreviousWeaponChoosing(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ChoosePreviousWeapon();
        }
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            float waitTime = 0f;
            if (_choosedWeapon.IsAbleToShoot())
            {
                _choosedWeapon.Shoot(Utility.GetCameraLookingPoint());
                WeaponStats weaponStats = _choosedWeapon.GetWeaponData().GetWeaponStats(_choosedWeapon.GetWeaponLevel());
                waitTime = weaponStats.ShootingStats.ShotDelay;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void ChooseNextWeapon()
    {
        int nextID = Array.IndexOf(_weapons, _choosedWeapon) + 1;
        int maxID = _weapons.Length - 1;

        if (nextID > maxID)
        {
            _choosedWeapon = _weapons[0];
        }
        else
        {
            _choosedWeapon = _weapons[nextID];
        }
    }

    private void ChoosePreviousWeapon()
    {
        int prevID = Array.IndexOf(_weapons, _choosedWeapon) - 1;
        int maxID = _weapons.Length - 1;

        if (prevID < 0)
        {
            _choosedWeapon = _weapons[maxID];
        }
        else
        {
            _choosedWeapon = _weapons[prevID];
        }
    }
}
