using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMove : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
    }

    private void SetDestination()
    {
        Vector3 targetVector = destination.transform.position;
        _navMeshAgent.SetDestination(targetVector);
        _animator.SetBool("walking", _navMeshAgent.velocity.magnitude > 0.4);

    }

    private void Update()
    {
        SetDestination();
    }
}
