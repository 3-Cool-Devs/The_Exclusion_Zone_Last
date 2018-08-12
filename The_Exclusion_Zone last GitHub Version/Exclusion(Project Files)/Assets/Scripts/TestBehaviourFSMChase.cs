﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviourFSMChase : TestBehaviourBase
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		base.OnStateEnter (animator, stateInfo, layerIndex);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        var direction = Player.transform.position - Head.transform.position;
        direction.y = 0;
		if (direction.magnitude < demonChaseRange) // if you enter it is chase range it chases you
		{ 
			myAgent.SetDestination (Player.transform.position);
            myAgent.speed = demonChaseSpeed;
            myAgent.acceleration = demonAcceleration;
        } 
		if (direction.magnitude > demonNoticeRange)
		{
            TDB.isLooking = true;
            TDB.uiBehav.hasBeenSpotted = false;
            PPB.profile.chromaticAberration.enabled = false;
            myAgent.speed = demonSpeed;
            myAgent.acceleration = demonSpeed;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
		
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	//{
	//Demon.transform.rotation = Quaternion.Slerp (Demon.transform.rotation, Quaternion.LookRotation (direction), chaseRotationSpeed * Time.deltaTime);
    //}
}