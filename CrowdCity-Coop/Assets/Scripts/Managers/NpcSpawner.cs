using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Player;
using Scriptables;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NpcSpawner : MonoBehaviour
{
    private List<GameObject> _npcWalkers = new List<GameObject>();
    private GameSettings _settings;
    
    private void Start()
    {
        _settings = GameResources.gm.settings;
        
        for (int i = 0; i < _settings.startAmountOfNpc; i++)
        {
            SpawnWalker();
        }

        StartCoroutine(ContinuesSpawner());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator ContinuesSpawner()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            
            if(_npcWalkers.Count < _settings.maxAmountOfNpc)
                SpawnWalker();
        }
    }

    
    private void SpawnWalker()
    {
        while (true)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(_settings.xSize.x, _settings.xSize.y), 1, Random.Range(_settings.zSize.x, _settings.zSize.y));

            if (NavMesh.SamplePosition(spawnPosition, out var hit, 50.0f, NavMesh.AllAreas))
            {
                Vector3 pos = hit.position;
                Npc go = Instantiate(_settings.npcPrefab, pos, Quaternion.identity, transform).GetComponent<Npc>();
                _npcWalkers.Add(go.gameObject);
                break;
            }
        }
    }

    public void SpawnFollowers(Leader l)
    {
        for (int i = 0; i < _settings.followersForCapturing; i++)
        {
            while (true)
            {
                Vector3 temp = l.transform.position + Random.insideUnitSphere * Random.Range(3, 10);

                if (NavMesh.SamplePosition(temp, out NavMeshHit hit, 50, NavMesh.AllAreas))
                {
                    Vector3 pos = hit.position;
                    Npc go = Instantiate(_settings.npcPrefab, pos, Quaternion.identity, transform).GetComponent<Npc>();
                    l.AddFollower(go);
                    
                    break;
                }
            }
        }
    }
}