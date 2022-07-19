using UnityEngine;
using TMPro;

public class EnemiesDisplay : MonoBehaviour
{
    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        GameEventHandler.EnemySpawned += UpdateValue;
        GameEventHandler.EnemyDied += UpdateValue;
    }

    private void UpdateValue(Enemy enemy)
    {
        display.text = $"ENM:{GameController.EnemiesCount}";
    }
}
