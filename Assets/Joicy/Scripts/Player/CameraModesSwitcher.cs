using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Zenject;

public class CameraModesSwitcher : MonoBehaviour
{
    [SerializeField] private BoolEventChannel gameStateChanged = null;

    [Inject] private SaveData saveData = null;
    [Inject] private SettingsData settingsData = null;
    [Inject] private ResourcesLoader resourcesLoader = null;

    private CameraModeUpgradeData[] _cameraModesData = null;
    private CameraModeUpgrade[] _cameraModes = null;
    private CameraActionMap _actionMap = null;
    private int _choosedMode = 0;
    private UniversalAdditionalCameraData _cameraData = null;

    private void Awake()
    {
        _actionMap = new CameraActionMap();
        _cameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();

        UpdateCameraModes();
    }

    private void OnEnable()
    {
        gameStateChanged.ChannelEvent += OnGameStateChanged;

        _actionMap?.Enable();
        _actionMap.Modes.ChooseNextMode.performed += SetNextMode;
        _actionMap.Modes.ChoosePreviousMode.performed += SetPreviousMode;
    }

    private void SetNextMode(InputAction.CallbackContext context)
    {
        if (_cameraModes != null && _cameraModes.Length > 0)
        {
            int nextID = _choosedMode + 1;
            int maxID = _cameraModesData.Length - 1;

            if (nextID > maxID)
            {
                _choosedMode = 0;
            }
            else
            {
                _choosedMode = nextID;
            }

            SetMode(_choosedMode);
        }
    }

    private void SetPreviousMode(InputAction.CallbackContext context)
    {
        if (_cameraModes != null && _cameraModes.Length > 0)
        {
            int prevID = _choosedMode - 1;
            int maxID = _cameraModesData.Length - 1;

            if (prevID < 0)
            {
                _choosedMode = maxID;
            }
            else
            {
                _choosedMode = prevID;
            }

            SetMode(_choosedMode);
        }
    }

    private void SetMode(int index)
    {
        CameraModeUpgradeData cameraModeData = _cameraModesData[index];
        int rendererIndex = cameraModeData.RendererDataIndex;
        _cameraData.SetRenderer(rendererIndex);

        ScriptableRendererFeature[] rendererFeatures = cameraModeData.RendererData.rendererFeatures.ToArray();
        foreach (ScriptableRendererFeature feature in rendererFeatures)
        {
            if (feature.GetType() == typeof(Blit))
            {
                Blit blit = (Blit)feature;
                if (blit && blit.name == "Interference")
                {
                    blit.SetActive(settingsData.Graphic.CameraNoise);
                    break;
                }
            }
        }
    }

    private void UpdateCameraModes()
    {
        List<CameraModeUpgradeData> cameraUpgradesDatas = new List<CameraModeUpgradeData>();
        List<CameraModeUpgrade> cameraUpgrades = new List<CameraModeUpgrade>();

        foreach (DroneUpgradeData upgradeData in resourcesLoader.DroneUpgrades)
        {
            if (upgradeData.GetType() == typeof(CameraModeUpgradeData))
            {
                CameraModeUpgradeData cameraUpgrade = (CameraModeUpgradeData)upgradeData;
                if (cameraUpgrade)
                {
                    saveData.UpgradesData.DroneLevels.TryGetValue(cameraUpgrade.Name, out int upgradeLevel);
                    if (upgradeLevel > 0)
                    {
                        cameraUpgradesDatas.Add(cameraUpgrade);
                        cameraUpgrades.Add((CameraModeUpgrade)cameraUpgrade.GetUpgrade(upgradeLevel));
                    }
                }
            }
        }

        _cameraModesData = cameraUpgradesDatas.ToArray();
        _cameraModes = cameraUpgrades.ToArray();
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