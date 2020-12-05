using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "NPC/Follower State")]
public class FollowerState : NpcState
{
    public float leaderOffset = 2;

    public override void Execute(Npc npc)
    {
        Vector3 offset = new Vector3(Random.Range(-leaderOffset, leaderOffset), 0, Random.Range(-leaderOffset, leaderOffset));
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
        npc.Agent.autoBraking = true;
        npc.Agent.stoppingDistance = 0.05f + (0.01f * npc.Leader.Followers);
    }
}