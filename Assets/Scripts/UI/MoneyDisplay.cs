using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        GameEventHandler.StatsChanged += UpdateValue;
    }

    private void UpdateValue()
    {
        display.text = $"$: {GameController.GetMoney()}";
    }
}
