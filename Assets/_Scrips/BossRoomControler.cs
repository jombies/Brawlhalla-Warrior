using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomControler : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject rewardChest;
    [SerializeField] GameObject gate;

    public List<GameObject> gates;

    private bool isStarted = false;
    private bool isCleared = false;
    private float offsetY = 2;

    public void GateClose()
    {

        isStarted = true;

        foreach (var gate in gates)
            gate.transform.DOMoveY(gate.transform.position.y + offsetY, 1f);
        if (boss.TryGetComponent<BossAI>(out var bossAI))
            bossAI.PlayerOnGr = true;
        if (boss.TryGetComponent<BossAI01>(out var bossAI01))
            bossAI01.PlayerOnGr = true;
        if (boss.TryGetComponent<BossAISpawner>(out var bossAIps))
            bossAIps.PlayerOnGr = true;
        boss.SetActive(true);
        EnemyStats bossHealth = boss.GetComponent<EnemyStats>();
        bossHealth.OnDeath += OnBossDefeated;
    }

    private void Reset()
    {
        foreach (Transform go in transform) {
            gates.Add(go.gameObject);
        }
    }
    void OnBossDefeated()
    {
        if (isCleared) return;
        isCleared = true;

        foreach (var gate in gates)
            gate.transform.DOMoveY(-2, 1f);

        rewardChest.SetActive(true);
        gate.SetActive(true);
    }
}


