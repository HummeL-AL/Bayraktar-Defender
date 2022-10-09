using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private VoidEventChannel speedChanged = null;
    [SerializeField] private BoolEventChannel gameStateChanged = null;

    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private float _accelerationSpeed = 0.5f;
    [SerializeField] private float _speed = 1f;

    [SerializeField] AudioSource _audioSource = null;
    [SerializeField] Vector2 _minMaxVolume = Vector2.one;

    private PlayerActionMap _actionMap = null;

    public float Speed { get => _speed; }

    public void SetStats(SpeedUpgrade stats)
    {
        _minSpeed = stats.MinSpeed;
        _maxSpeed = stats.MaxSpeed;
        _accelerationSpeed = stats.Acceleration;
    }

    private void Awake()
    {
        gameStateChanged.ChannelEvent += OnGameStateChanged;

        _actionMap = new PlayerActionMap();
    }

    private void OnEnable()
    {
        _actionMap?.Enable();
    }

    private void Update()
    {
        Rotate();
        ChangeSpeed(_actionMap.Movement.Accelerating.ReadValue<float>());
    }

    private void Rotate()
    {
        transform.parent.rotation *= Quaternion.Euler(0f, -_speed * Time.deltaTime, 0f);
    }

    private void ChangeSpeed(float delta)
    {
        _speed = Mathf.Clamp(_speed + (delta * _accelerationSpeed * Time.deltaTime), _minSpeed, _maxSpeed);

        float volumePercent = (_speed - _minSpeed) / (_maxSpeed - _minSpeed);
        _audioSource.volume = _minMaxVolume.x + (_minMaxVolume.y - _minMaxVolume.x) * volumePercent;

        speedChanged.RaiseEvent();
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
