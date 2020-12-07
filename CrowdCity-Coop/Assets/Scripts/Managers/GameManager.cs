using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Player;
using Scriptables;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameSettings settings;
    public PlayerInputManager inputManager;
    
    public List<Transform> spawnSpots = new List<Transform>();
    private List<Leader> _leaders = new List<Leader>();

    private PlayerInputManager _inputManager;
    
    public void Awake()
    {
        GameResources.gm = this;
        _inputManager = FindObjectOfType<PlayerInputManager>();
        
        /*SpawnLeaders();*/
    }
    
    private void SpawnLeaders()
    {
        /*for (var i = 0; i < settings.players.Count; i++)
        {
            PlayerSetting player = settings.players[i];
            Leader p = SpawnLeader(settings.playerPrefab, player.color);
            inputManager.JoinPlayer(i, i, "Player", player.devices.ToArray());
            
            PlayerMover mover = p.GetComponent<PlayerMover>();
            mover.PlayerIndex = i;
        }*/

        while (_leaders.Count < settings.maxAmountOfPlayers)
        {
            var c = settings.RandomSkinTable();
            c.taken = true;

            /*SpawnLeader(settings.aiPrefab, c.value);*/
        }
    }

    private Leader SpawnLeader(GameObject prefab, Color c)
    {
        Transform random = spawnSpots[Random.Range(0, spawnSpots.Count)];
        spawnSpots.Remove(random);
        
        Leader l = Instantiate(prefab, random.position, Quaternion.identity, transform).GetComponent<Leader>();
        _leaders.Add(l);
        
        return l;
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