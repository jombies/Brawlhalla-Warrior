using System.Collections;
using UnityEngine;

public class nextLevev : MonoBehaviour
{
    [SerializeField] string nextLevelName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            SceneLoaderNew.i.loadScene(nextLevelName/*, () => { GameManager.Instance.SpawnAfterSceneLoaded(); }*/);
        }
    }
}
