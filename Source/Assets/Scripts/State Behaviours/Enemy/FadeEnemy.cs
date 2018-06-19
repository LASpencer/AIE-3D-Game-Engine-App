using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEnemy : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Enemy e = animator.GetComponent<Enemy>();
        if(e != null)
        {
            foreach(Renderer r in e.irRenderers)
            {
                r.enabled = false;
            }

            foreach(Renderer r in e.defaultRenderers)
            {
                r.material.SetFloat("_Mode", 3);
            }
        }
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Enemy e = animator.GetComponent<Enemy>();
        if (e != null)
        {

            foreach (Renderer r in e.defaultRenderers)
            {
                Color newColour = r.material.color;

                newColour.a = Mathf.Max(0, newColour.a - Time.deltaTime * 0.5f);
                r.material.color = newColour;
            }
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
