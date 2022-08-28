using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public int Money { get; private set; }
    public int Victims { get; private set; }
    public int Wave { get; private set; }
    public int MaxWave { get; private set; }
    public int Enemies { get; private set; }

    [SerializeField] private VoidEventChannel moneyUpdated = null;
    [SerializeField] private VoidEventChannel victimsUpdated = null;
    [SerializeField] private VoidEventChannel waveChanged = null;
    [SerializeField] private VoidEventChannel enemiesChanged = null;
    
    public void AddMoney(int increase)
    {
        Money += increase;
        moneyUpdated.RaiseEvent();
    }

    public void AddVictims(int increase)
    {
        Victims += increase;
        victimsUpdated.RaiseEvent();
    }

    public void IncreaseWave()
    {
        Wave++;
        waveChanged.RaiseEvent();
    }

    public void SetMaxWave(int wave)
    {
        MaxWave = wave;
    }

    public void AddEnemies(int increase)
    {
        Enemies += increase;
        enemiesChanged.RaiseEvent();
    }
}
