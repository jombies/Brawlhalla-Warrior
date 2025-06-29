using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PracticeDummy : MonoBehaviour
{
    [SerializeField] GameObject PopUpDame;
    [SerializeField] TextMeshPro _textDamePopup;
    GameObject player;
    CharacterAnimation playerAnim;
    PlayerStat playerStat;
    Vector3 PopupOffset = new Vector3(0, 2.5f, 0);
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerReferences.Instance.Player;
        if (player != null) {
            playerAnim = player.transform.GetChild(0).GetComponent<CharacterAnimation>();
            playerStat = player.GetComponent<PlayerStat>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("weapon") && playerAnim.IsAttacking) {
            _textDamePopup.text = (playerStat.Damage.Value * -1).ToString();
            Instantiate(PopUpDame, transform.position + PopupOffset, Quaternion.identity);
        }
    }
}
