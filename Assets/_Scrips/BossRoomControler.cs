using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomControler : MonoBehaviour
{
    public GameObject boss;
    public GameObject rewardChest;
    public List<GameObject> gates;

    private bool isStarted = false;
    private bool isCleared = false;

    public void GateClose()
    {

        isStarted = true;

        foreach (var gate in gates)
            gate.transform.DOMoveY(1, 1f); // đóng cửa
        if (boss.TryGetComponent<BossAI>(out var bossAI))
            bossAI.PlayerOnGr = true; // thông báo boss đã sẵn sàng chiến đấu
        if (boss.TryGetComponent<BossAI01>(out var bossAI01))
            bossAI01.PlayerOnGr = true; // thông báo boss đã sẵn sàng chiến đấu
        if (boss.TryGetComponent<BossAISpawner>(out var bossAIps))
            bossAIps.PlayerOnGr = true; // thông báo boss đã sẵn sàng chiến đấu
        boss.SetActive(true); // hiện boss nếu ẩn
                              //BossHealth bossHealth = boss.GetComponent<BossHealth>();
                              // bossHealth.OnBossDead += OnBossDefeated;
    }
}

//void OnBossDefeated()
//{
//    if (isCleared) return;
//    isCleared = true;

//    foreach (var gate in gates)
//        gate.transform.DOMoveY(-2, 1f); // mở cửa

//    rewardChest.SetActive(true);
//    Debug.Log("🎉 Boss defeated!");
//}
//}
