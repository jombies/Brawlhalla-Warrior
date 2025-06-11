
using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    public enum StringOption
    {
        Slash1,
        Slash2,
        Slash3
    }

    public StringOption selectedOption;

    public string GetSelectedString()
    {
        switch (selectedOption) {
            case StringOption.Slash1:
                return "enemy slash1";
            case StringOption.Slash2:
                return "enemy slash2";
            case StringOption.Slash3:
                return "Chuỗi 3";
            default:
                return "Không xác định";
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.Instance.PlaySFX(GetSelectedString());
    }
}