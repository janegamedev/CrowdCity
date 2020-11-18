using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Player;
using Scriptables;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameSettings settings;
    
    public List<Transform> spawnSpots = new List<Transform>();
    private List<Leader> _leaders = new List<Leader>();
    private List<Color> _leaderColors = new List<Color>();
  
    public void Awake()
    {
        GameResources.gm = this;

        _leaderColors = settings.leaderColors.ToList();
        Debug.Log(_leaderColors.Count);
        SpawnLeaders();
    }
    
    private void SpawnLeaders()
    {
        while (_leaders.Count < settings.maxAmountOfPlayers)
        {
            Transform random = spawnSpots[Random.Range(0, spawnSpots.Count)];

            Leader l = Instantiate(settings.playerPrefab, random.position, Quaternion.identity, transform).GetComponent<Leader>();
            Color c = _leaderColors[Random.Range(0,_leaderColors.Count)];
            l.SetLeader(c);

            _leaderColors.Remove(c);
            _leaders.Add(l);

            spawnSpots.Remove(random);
        }
    }

    public void DestroyLeader(Leader l)
    {
        _leaders.Remove(l);
        Destroy(l.gameObject);

        if (_leaders.Count < 2)
            OnGameEnd();
    }

    private void OnGameEnd()
    {
        Debug.Log("Game Over");
    }
}

[System.Serializable]
public class ColorLeader
{
    public Color color;
    public bool isTaken;
}