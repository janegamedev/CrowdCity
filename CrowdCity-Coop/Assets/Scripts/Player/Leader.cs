using System;
using UnityEngine;

namespace Player
{
    public class Leader: MonoBehaviour
    {
        public LayerMask layerMask;
        private int _followers = 1;
        private Color _leaderColor;

        public int Followers
        {
            get => _followers;
            set => _followers = value;
        }

        public Color LeaderColor => _leaderColor;

        public void SetLeader(Color c)
        {
            _leaderColor = c;
            GetComponentInChildren<Renderer>().material.SetColor("_BaseColor", _leaderColor);
        }

        public void KillPlayer(Leader leader)
        {
            FindObjectOfType<GameManager>().DestroyLeader(leader);
            FindObjectOfType<NpcSpawner>().SpawnFollowers(this);
        }

        public void AddFollower(Npc npc)
        {
            npc.SetLeader(this);
            _followers++;
        }

        private void Update()
        {
            CheckCollider();
        }

        private void CheckCollider()
        {
            foreach (Collider col in Physics.OverlapSphere(transform.position, 2, layerMask))
            {
                if (col.TryGetComponent(out Npc npc))
                {
                    if(npc.IsWalker || npc.Leader != this && npc.Leader.Followers < _followers)
                        AddFollower(npc);
                    
                    continue;
                }
                
                if (col.TryGetComponent(out Leader leader))
                {
                    if(leader != this && leader.Followers == 1)
                        KillPlayer(leader);
                }
            }
        }
    }
}