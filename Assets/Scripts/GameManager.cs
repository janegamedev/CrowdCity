using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    float timeLeft=3;

    public GameObject npc;
    public GameObject player;
    public GameObject stat;
    public GameObject statParent;

    public List<GameObject> players;
    public List<GameObject> statistics;
    public List<GameObject> spawnPositions;

    private void Start()
    {
        SpawnNpc();
        SpawnPlayer();
    }

    private void Update()
    {
        Timer();
        UpdateStat();
    }

    void Timer()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft<=0)
        {
            SpawnNpc();
            timeLeft = 3;
        }
    }

    void SpawnNpc()
    {
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            Instantiate(npc, spawnPositions[i].transform.position, Quaternion.identity);
            Instantiate(npc, spawnPositions[i].transform.position, Quaternion.identity);
        }
    }

    void SpawnPlayer()
    {
        GameObject go=Instantiate(player);
        players.Add(go);
        SpawnStatistic();
    }

    void SpawnStatistic()
    {
        GameObject go = Instantiate(stat,statParent.transform);
        statistics.Add(go);
    }

    void UpdateStat()
    {
        for (int i = 0; i < statistics.Count; i++)
        {
            statistics[i].GetComponentInChildren<RawImage>().color = players[i].GetComponent<Renderer>().material.color;
            statistics[i].GetComponentInChildren<TextMeshProUGUI>().text = players[i].GetComponent<PlayerMovement>().amount.ToString();
        }
    }

}
