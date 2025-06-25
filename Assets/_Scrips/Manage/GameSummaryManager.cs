using UnityEngine;

public class GameSummaryManager : MonoBehaviour
{
    public static GameSummaryManager Instance;

    public int totalEnemyKilled = 0;
    public int totalBossKilled = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void ResetSession()
    {
        totalEnemyKilled = 0;
        totalBossKilled = 0;
    }

    public void AddEnemyKill() => totalEnemyKilled++;
    public void AddBossKill() => totalBossKilled++;
}
