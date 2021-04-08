using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chicken : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public Transform[] cpoints;
    int m_CurrentcpointIndex;
    static Animator anim;

    void Start()
    {
        navMeshAgent.SetDestination(cpoints[0].position);

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentcpointIndex = (m_CurrentcpointIndex + 1) % cpoints.Length;
            navMeshAgent.SetDestination(cpoints[m_CurrentcpointIndex].position);
        }
    }
}
