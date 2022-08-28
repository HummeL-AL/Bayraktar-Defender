using UnityEngine;
using Zenject;
using TMPro;

public class TotalMoneyDisplay : MonoBehaviour
{
    [SerializeField] private VoidEventChannel moneyChangedChannel = null;

    [Inject] private SaveData saveData = null;
    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        moneyChangedChannel.ChannelEvent += UpdateValue;
    }

    private void Start()
    {
        UpdateValue();
    }

    private void UpdateValue()
    {
        display.text = $"$: {saveData.PlayerData.Money}";
    }

    private void OnDestroy()
    {
        moneyChangedChannel.ChannelEvent -= UpdateValue;
    }
}