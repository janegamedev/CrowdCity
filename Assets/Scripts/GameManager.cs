using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    float timer;

    public GameObject npc;
    public GameObject player;
    public GameObject playerAi;
    public GameObject stat;
    public GameObject statParent;
    public GameObject buildingsRoot;

    public Camera cam;

    public List<GameObject> players;
    public List<GameObject> statistics;
    public List<GameObject> playersSpawnPos;
    public List<GameObject> npcWalker;

    public static GameManager GM;


    private void Start()
    {
        GM = this;
        for (int i = 0; i < 150; i++)
        {
            SpawnNpc();
        }
        SpawnPlayer();
    }

    private void Update()
    {
        if (timer > 1 && npcWalker.Count < 200)
        {
            SpawnNpc();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }

        UpdateStat();
    }

    void SpawnNpc()
    {
        Vector3 pos = new Vector3(Random.Range(2, 198), 1, Random.Range(2, 198));

        Vector3 temp = FindPoint(pos);
        if (temp == Vector3.zero)
        {
            SpawnNpc();
        }
        else
        {
            GameObject go = Instantiate(npc, temp, Quaternion.identity);
            npcWalker.Add(go);
        }
    }

    public void SpawnFollowers(GameObject _player)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = _player.transform.position + Random.insideUnitSphere * Random.Range(3, 10);

            Vector3 temp = FindPoint(pos);
            if (temp == Vector3.zero)
            {
                i -= 1;
                continue;
            }
            else
            {
                GameObject go = Instantiate(npc, temp, Quaternion.identity);
                go.GetComponent<NpcController>().AddPlayer(_player);
            }
        }
        _player.GetComponent<Agent>().amount += 10;
    }

    Vector3 FindPoint(Vector3 pos)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 50.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void SpawnPlayer()
    {
        for (int i = 0; i < 12; i++)
        {
            if (i == 0)
            {
                GameObject go = Instantiate(player, playersSpawnPos[i].transform.position, Quaternion.identity);
                cam.GetComponent<CameraFollow>().player = go;
                players.Add(go);
                SpawnStatistic();
            }
            else
            {
                GameObject go = Instantiate(playerAi, playersSpawnPos[i].transform.position, Quaternion.identity);
                players.Add(go);
                SpawnStatistic();
            }
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

    public void RemoveNpcFromArray(GameObject _npc)
    {
        if (npcWalker.Contains(_npc))
        {
            npcWalker.RemoveAt(npcWalker.IndexOf(_npc));
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
