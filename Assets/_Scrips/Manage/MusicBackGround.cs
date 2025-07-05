using UnityEngine;

public class MusicBackGround : MonoBehaviour
{
    [SerializeField] string musicName;
    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(musicName)) {
            AudioManager.Instance.PlayMusic(musicName);
        }
    }
}
