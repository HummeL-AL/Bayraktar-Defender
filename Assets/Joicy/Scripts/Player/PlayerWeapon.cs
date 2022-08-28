using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private VoidEventChannel weaponSwitchedChannel = null;

    [SerializeField] private List<Weapon> _weapons = null;

    private Weapon _choosedWeapon = null;
    public Weapon CurrentWeapon { get => _choosedWeapon; }

    public void SetWeapon(int index)
    {
        _choosedWeapon = _weapons[index];
        weaponSwitchedChannel.RaiseEvent();
    }

    public void AddWeapon(Weapon weapon)
    {
        _weapons.Add(weapon);
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
            if (_choosedWeapon.IsAbleToShoot)
            {
                _choosedWeapon.Shoot(Utility.GetCameraLookingPoint());
                WeaponStats weaponStats = _choosedWeapon.WeaponStats;
                waitTime = weaponStats.ShootingStats.ShotDelay;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void ChooseNextWeapon()
    {
        int nextID = _weapons.IndexOf(_choosedWeapon) + 1;
        int maxID = _weapons.Count - 1;

        if (nextID > maxID)
        {
            _choosedWeapon = _weapons[0];
        }
        else
        {
            _choosedWeapon = _weapons[nextID];
        }

        weaponSwitchedChannel.RaiseEvent();
    }

    private void ChoosePreviousWeapon()
    {
        int prevID = _weapons.IndexOf(_choosedWeapon) - 1;
        int maxID = _weapons.Count - 1;

        if (prevID < 0)
        {
            _choosedWeapon = _weapons[maxID];
        }
        else
        {
            _choosedWeapon = _weapons[prevID];
        }

        weaponSwitchedChannel.RaiseEvent();
    }
}
