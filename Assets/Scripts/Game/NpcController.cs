using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private int _waiting;
    public bool isWalking;

    private PhotonView _view;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _view = GetComponent<PhotonView>();
        NavMesh.avoidancePredictionTime = 0.5f;
        //_navMeshAgent.speed = Random.Range(1.0f, 2.5f);
        if (!_view.IsMine) return;
        CalculateNewPath();
        StartCoroutine(SyncPosition());
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        _animator.SetBool("walking", _navMeshAgent.velocity.magnitude > 0.6);
        if (_view.IsMine)
        {
            FindPath();
        }
    }

    IEnumerator SyncPosition()
    {
        for (;;)
        {
            _view.RPC("SetPosition", RpcTarget.Others, transform.position);
            yield return new WaitForSeconds(1);
        }
    }

    [PunRPC]
    private void SetPosition(Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) > 2)
        {
            transform.position = pos;
            Debug.Log("WURDE RUMTELEPORTIERT");
        }
    }

    [PunRPC]
    private void SetDestination(Vector3 pos, Vector3 destination)
    {
        transform.position = pos;
        _navMeshAgent.destination = destination;
    }

    private void FindPath()
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
            }
        }
    }

    private void CalculateNewPath()
    {
        var destination = NpcSpawner.Instance.RandomShopFloorTile(gameObject.transform.position);
        _navMeshAgent.destination = destination;
        _view.RPC("SetDestination", RpcTarget.Others, transform.position, destination);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}