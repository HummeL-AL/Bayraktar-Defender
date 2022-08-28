using UnityEngine;
using TMPro;

public class SpeedDisplay : MonoBehaviour
{
    [SerializeField] private PlayerMoving _player = null;

    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
    }

    public void UpdateValue()
    {
        string speed = string.Format("{0:0.0}", _player.Speed);
        display.text = $"SPD:{speed}";
    }
}
