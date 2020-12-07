using System;
using System.Collections.Generic;
using System.Linq;
using Scriptables;
using UnityEngine;

namespace Player
{
    public class Leader: MonoBehaviour
    {
        public LayerMask layerMask;
        public BoolVariable checkCollider;
        public List<SkinObject> skins;
        
        private int _followers = 1;
        private Skin _currentSkin;
        private SkinObject _currentSkinObject;
        private CharacterController _controller;
        private PlayerConfiguration _playerConfiguration;
        private bool _isInit;

        public int Followers
        {
            get => _followers;
            set => _followers = value;
        }

        public Skin CurrentSkin 
        {
            get => _currentSkin;
            private set
            {
                if (_currentSkin != null && _currentSkin != value)
                {
                    _currentSkinObject.go.SetActive(false);    
                }
                
                _currentSkin = value;
                
                if(_currentSkin == null) return;
      
                _currentSkinObject = skins.FirstOrDefault(x => x.skinType == _currentSkin);
                _currentSkinObject.go.SetActive(true);
            }
        }
        public int LeaderboardPlace { get; set; }
        public PlayerConfiguration PlayerConfigurations => _playerConfiguration;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void SetConfiguration(PlayerConfiguration config)
        {
            _playerConfiguration = config;
            CurrentSkin = _playerConfiguration.skin;
            _playerConfiguration.onSkinUpdated += UpdateSkin;
            _playerConfiguration.onNicknameUpdated += UpdateNick;
        }

        private void UpdateSkin()
        {
            CurrentSkin = _playerConfiguration.skin;
        }

        private void UpdateNick()
        {
            
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
            if(!checkCollider.value) return;
            
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

    [System.Serializable]
    public class SkinObject
    {
        public GameObject go;
        public Skin skinType;
    }
}