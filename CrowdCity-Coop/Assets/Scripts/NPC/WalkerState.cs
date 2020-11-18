using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "NPC/Walker State")]
public class WalkerState : NpcState
{
    public int minDistance, maxDistance;
 
    public override void Execute(Npc npc)
    {
        if(npc.ReachedDestination)
        {
            while (true)
            {
                Vector3 random = npc.transform.position + new Vector3(Random.Range(-maxDistance, maxDistance), 0,
                    Random.Range(-maxDistance, maxDistance));
                
                if (NavMesh.SamplePosition(random, out var hit, 50.0f, NavMesh.AllAreas))
                {
                    npc.Destination = hit.position;
                    break;
                }
            }
        }
    }

    public override void OnStateEnd(Npc npc)
    {
        //GameManager.GM.RemoveNpcFromArray(gameObject);
    }
    
  
}