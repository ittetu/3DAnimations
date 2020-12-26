using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class State : StateMachineBehaviour
{
    public delegate void CallBack();

    public CallBack onStateEnter;
    public CallBack onStateExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateEnter();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateExit();
    }

    
}
