using UnityEngine;
using TMPro;

public class WaveDisplay : MonoBehaviour
{
    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        GameEventHandler.WaveChanged += UpdateValue;
    }

    private void UpdateValue()
    {
        display.text = $"WAVE:{GameController.GetWave()}";
    }
}
