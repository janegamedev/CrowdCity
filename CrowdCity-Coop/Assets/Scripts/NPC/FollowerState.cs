using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "NPC/Follower State")]
public class FollowerState : NpcState
{
    [NonSerialized] private float _leaderOffset;
    
    public override void Execute(Npc npc)
    {
        Vector3 offset = new Vector3(Random.Range(-_leaderOffset, _leaderOffset), 0, Random.Range(-_leaderOffset, _leaderOffset));
        npc.Destination = npc.Leader.transform.position + offset;
        npc.CheckCollider();
    }

    public override void OnStateEnd(Npc npc)
    {
        // remove from old player
    }

    public override void OnNewLeader(Npc npc)
    {
        npc.Leader.Followers--;
        npc.Agent.stoppingDistance = Mathf.RoundToInt(npc.Leader.Followers/2);
        _leaderOffset = Mathf.RoundToInt(npc.Leader.Followers/5);
    }
}