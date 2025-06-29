using UnityEngine;

public class CompleteTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            GameManager.Instance.OnPlayerWin();
            Debug.Log("Player has completed the level!");
        }
    }
}
