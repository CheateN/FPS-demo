using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehavior : StateMachineBehaviour
{
    private float timer;
    private List<Transform> wayPoints = new List<Transform>();
    private NavMeshAgent agent;
    private float chaseRange = 15f;
    private Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;

        Transform wayPointsObject = GameObject.FindWithTag("WayPoints").transform;
        foreach (Transform wayPoint in wayPointsObject)
        {
            wayPoints.Add(wayPoint);
        }

        agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.SetDestination(wayPoints[0].position);
        
        player = GameObject.FindWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > 10)
        {
            animator.SetBool("isPatrolling", false);
        }
        
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 将agent的目标位置设置为Animator的transform位置
        // 就是让agent的目标位置为当前位置
        agent.destination = animator.transform.position;
        agent.isStopped = true;
        // agent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}