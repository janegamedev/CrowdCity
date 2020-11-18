using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMover : MonoBehaviour
    {
        public int maxDistance;
        private NavMeshAgent _agent;
        private Vector3 _destination;
        private bool ReachedDestination => _agent.velocity.magnitude < .3f;

        private Vector3 Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                _agent.SetDestination(_destination);
            }
        }
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if(ReachedDestination)
            {
                while (true)
                {
                    Vector3 random = transform.position + new Vector3(Random.Range(-maxDistance, maxDistance), 0,
                        Random.Range(-maxDistance, maxDistance));
                
                    if (NavMesh.SamplePosition(random, out var hit, 50.0f, NavMesh.AllAreas))
                    {
                        Destination = hit.position;
                        break;
                    }
                }
            }
        }
    }
}