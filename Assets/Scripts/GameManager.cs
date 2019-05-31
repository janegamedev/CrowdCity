using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    float timeLeft = 3;

    public GameObject npc;
    public GameObject player;
    public GameObject playerAi;
    public GameObject stat;
    public GameObject statParent;

    public Camera cam;

    public List<GameObject> players;
    public List<GameObject> statistics;
    public List<GameObject> spawnPositions;

    public static GameManager GM;


    private void Start()
    {
        GM = this;
        SpawnNpc();
        SpawnPlayer();
        SpawnAI();
    }

    private void Update()
    {
        Timer();
        UpdateStat();
    }

    void Timer()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
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
        GameObject go = Instantiate(player);
        cam.GetComponent<CameraFollow>().player = go;
        players.Add(go);
        SpawnStatistic();
    }

    void SpawnAI()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(playerAi);
            players.Add(go);
            SpawnStatistic();
        }
    }

    void SpawnStatistic()
    {
        GameObject go = Instantiate(stat, statParent.transform);
        statistics.Add(go);
    }

    void UpdateStat()
    {
        for (int i = 0; i < statistics.Count; i++)
        {
            statistics[i].GetComponentInChildren<RawImage>().color = players[i].GetComponentInChildren<Renderer>().material.color;
            statistics[i].GetComponentInChildren<TextMeshProUGUI>().text = players[i].GetComponent<Agent>().amount.ToString();
        }
    }

    public void DestroyPlayer(GameObject _player)
    {
        Destroy(statistics[players.IndexOf(_player)].gameObject);
        statistics.RemoveAt(players.IndexOf(_player)); 

        players.RemoveAt(players.IndexOf(_player));
        Destroy(_player);


    }
}
