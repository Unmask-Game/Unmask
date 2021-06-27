using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{

    private NavMeshAgent _navMeshAgent;
    private int _waiting;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        NavMesh.avoidancePredictionTime = 0.5f;
        _navMeshAgent.speed = Random.Range(1.0f, 2.5f);
        CalculateNewPath();
    }
    
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_waiting == 0)
        {
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        _waiting = Random.Range(50, 150);
                    }
                }
            }
        }
        else
        {
            if (--_waiting == 0)
            {
                CalculateNewPath();
            };
        }
    }

    private void CalculateNewPath()
    {
        _navMeshAgent.destination = NpcSpawner.Instance.RandomShopFloorTile(gameObject.transform.position);
    }
}
