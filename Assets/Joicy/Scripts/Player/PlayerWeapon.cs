using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private BoolEventChannel gameStateChanged = null;
    [SerializeField] private VoidEventChannel weaponSwitchedChannel = null;

    private PlayerActionMap _actionMap = null;
    private List<Weapon> _weapons = new List<Weapon>();
    private Weapon _choosedWeapon = null;
    private Coroutine shootRoutine = null;

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

    private void Awake()
    {
        gameStateChanged.ChannelEvent += OnGameStateChanged;

        _actionMap = new PlayerActionMap();
        _actionMap.Weapons.ChooseNextWeapon.performed += ChooseNextWeapon;
        _actionMap.Weapons.ChoosePreviousWeapon.performed += ChoosePreviousWeapon;

        _actionMap.Weapons.Shooting.started += CheckShooting;
        _actionMap.Weapons.Shooting.canceled += CheckShooting;
    }

    private void OnEnable()
    {
        _actionMap?.Enable();
    }

    private void CheckShooting(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            shootRoutine = StartCoroutine(Shooting());
        }
        else if(context.canceled)
        {
            StopCoroutine(shootRoutine);
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

    private void ChooseNextWeapon(InputAction.CallbackContext context)
    {
        int nextID = _weapons.IndexOf(_choosedWeapon) + 1;
        int maxID = _weapons.Count - 1;

        if (nextID > maxID)
        {
            SetWeapon(0);
        }
        else
        {
            SetWeapon(nextID);
        }

        weaponSwitchedChannel.RaiseEvent();
    }

    private void ChoosePreviousWeapon(InputAction.CallbackContext context)
    {
        int prevID = _weapons.IndexOf(_choosedWeapon) - 1;
        int maxID = _weapons.Count - 1;

        if (prevID < 0)
        {
            SetWeapon(maxID);
        }
        else
        {
            SetWeapon(prevID);
        }

        weaponSwitchedChannel.RaiseEvent();
    }

    private void OnGameStateChanged(bool resumed)
    {
        if (resumed)
        {
            _actionMap?.Enable();
        }
        else
        {
            _actionMap?.Disable();
        }
    }

    private void OnDisable()
    {
        _actionMap?.Disable();
    }
}
