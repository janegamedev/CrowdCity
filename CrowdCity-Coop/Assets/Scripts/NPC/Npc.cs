using System;
using Player;
using UnityEngine;
using UnityEngine.AI;

public class Npc : MonoBehaviour
{
    public NpcState walkerState, followerState;
    public LayerMask layerMask;

    private NpcState _currentState;
    private Leader _leader;
    private NavMeshAgent _agent;
    private Vector3 _destination;
    private Renderer _renderer;

    #region Properties
    
    public bool IsWalker => _currentState == walkerState;
    public Leader Leader
    {
        get => _leader;
        set
        {
            _leader = value;
            _renderer.material.color = _leader.LeaderColor;
        }
    }

    public Vector3 Destination
    {
        get => _destination;
        set
        {
            _destination = value;
            _agent.SetDestination(_destination);
        }
    }

    public bool ReachedDestination => _agent.velocity == Vector3.zero;

    public NavMeshAgent Agent => _agent;

    #endregion
    
    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        Destination = transform.position;

        _renderer = GetComponent<Renderer>();

        _currentState = walkerState;
        _currentState.OnStateStart(this);
    }

    private void Update()
    {
        _currentState.Execute(this);
    }

    public void SetLeader(Leader lead)
    {
        if (_currentState != followerState)
        {
            _currentState.OnStateEnd(this);
            _currentState = followerState;
            _currentState.OnStateStart(this);
        }
        else
        {
            _currentState.OnNewLeader(this);
        }

        Leader = lead;
    }

    public void CheckCollider()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, 2, layerMask))
        {
            if (col.TryGetComponent(out Npc npc))
            {
                if(npc.IsWalker || npc.Leader != npc._leader && npc.Leader.Followers < Leader.Followers)
                    Leader.AddFollower(npc);
                    
                continue;
            }
                
            if (col.TryGetComponent(out Leader leader))
            {
                if(leader.Followers == 1)
                    _leader.KillPlayer(leader);
            }
        }
    }
}