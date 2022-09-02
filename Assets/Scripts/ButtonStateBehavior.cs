using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStateBehavior : StateMachineBehaviour
{
    public delegate void SoundDelegate();
    public static event SoundDelegate OnDown;
    public static event SoundDelegate OnUp;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<FloorButton>().ButtonDown();
        if (OnDown != null)
        {
            OnDown();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<FloorButton>().ButtonUp();
        if (OnUp != null)
        {
            OnUp();
        }
    }
}
