using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public List<Color> colors = new List<Color>();

    float timer;

    public GameObject npc;
    public GameObject player;
    public GameObject playerAi;
    public GameObject stat;
    public GameObject point;
    public TextMeshProUGUI timerUI;
 
    public GameObject buildingsRoot;
    public GameObject playersRoot;
    public GameObject npcRoot;
    public GameObject statRoot;
    public GameObject pointsRoot;

    public Camera cam;

    public List<GameObject> players;
    public List<GameObject> statistics;
    public List<GameObject> playersSpawnPos;
    public List<GameObject> npcWalker;
    public List<GameObject> points;

    public static GameManager GM;


    private void Start()
    {
        GM = this;

        timer = 0;

        colors = new List<Color> {new Color(0, 0, .5f), new Color(0, .5f, .5f), new Color(.5f, 0, .5f), new Color(0, .5f, 0),
            new Color(.5f, .5f, 0), new Color(.5f, 0, 0), new Color(.5f, .5f, .5f), new Color(1, 0, 1), new Color(0, 1, 1),
            new Color(1, 1, 0),new Color(0, 0, 1),new Color(0, 1, 0),new Color(1, 0, 0) };

        for (int i = 0; i < 100; i++)
        {
            SpawnNpc();
        }
        SpawnPlayer();
    }

    private void Update()
    {
        if (npcWalker.Count < 200)
        {
            SpawnNpc();
        }

        if (players.Count <= 1 || timer >= 30)
        {
            GameOver();
        }
        else
        {
            timer += Time.deltaTime;
            timerUI.text = Mathf.RoundToInt(timer).ToString();
        }
    }

    void SpawnNpc()
    {
        Vector3 temp = new Vector3(Random.Range(2, 198), 1, Random.Range(2, 198));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(temp, out hit, 50.0f, NavMesh.AllAreas))
        {
            Vector3 pos = hit.position;
            GameObject go = Instantiate(npc, pos, Quaternion.identity, npcRoot.transform);
            npcWalker.Add(go);
        }
        else
        {
            SpawnNpc();
        }
    }

    public void SpawnFollowers(GameObject _player)
    {
        int max = 10;

        for (int i = 0; i < max; i++)
        {
            Vector3 temp = _player.transform.position + Random.insideUnitSphere * Random.Range(3, 10);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(temp, out hit, 50.0f, NavMesh.AllAreas))
            {
                Vector3 pos = hit.position;

                GameObject go = Instantiate(npc, pos, Quaternion.identity, npcRoot.transform);
                go.GetComponent<NpcController>().currentState = NpcController.State.Following;
                go.GetComponent<NpcController>().AddPlayer(_player);
            }
            else
            {

                max++;
                continue;
            }
        }
        _player.GetComponent<Agent>().amount += 10;
    }

    void SpawnPlayer()
    {
        for (int i = 0; i < 12; i++)
        {
            if (i == 0)
            {
                GameObject go = Instantiate(player, playersSpawnPos[i].transform.position, Quaternion.identity, playersRoot.transform);
                cam.GetComponent<CameraFollow>().player = go;
                players.Add(go);

                GameObject go2 = Instantiate(point, pointsRoot.transform, false);
                go2.GetComponent<PointUpdate>().player = go;
                points.Add(go2);
            }
            else
            {
                GameObject go = Instantiate(playerAi, playersSpawnPos[i].transform.position, Quaternion.identity, playersRoot.transform);
                players.Add(go);

                GameObject go2 = Instantiate(point, pointsRoot.transform, false);
                go2.GetComponent<PointUpdate>().player = go;
                points.Add(go2);
            }
            players[i].GetComponent<Agent>().color = colors[i];
            
        }
        SpawnStatistic();
    }

    void SpawnStatistic()
    {
        for (int i = 0; i < players.Count; i++)
        {
            GameObject go = Instantiate(stat, statRoot.transform);
            statistics.Add(go);
        }
    }

    public void UpdateStat()
    {
        SortArray();
        for (int i = 0; i < statistics.Count; i++)
        {
            Color temp = players[i].GetComponentInChildren<Renderer>().material.color;
            statistics[i].GetComponentInChildren<RawImage>().color = new Color(temp.r,temp.g,temp.b,1);
            statistics[i].GetComponentInChildren<TextMeshProUGUI>().text = players[i].GetComponent<Agent>().amount.ToString();
        }
    }

    void SortArray()
    {
        players = players.OrderBy(x => x.GetComponent<Agent>().amount).ToList();
        players.Reverse();
    }

    public void RemoveNpcFromArray(GameObject _npc)
    {
        npcWalker.RemoveAt(npcWalker.IndexOf(_npc));
    }

    public void DestroyPlayer(GameObject _player)
    {
        int temp = players.IndexOf(_player);

        Destroy(points[temp].gameObject);
        Destroy(statistics[temp].gameObject);
        Destroy(_player.gameObject);
        statistics.RemoveAt(temp);
        players.RemoveAt(temp);
        points.RemoveAt(temp);
        
    }

    public void GameOver()
    {
        foreach (Transform child in playersRoot.transform)
        {
            Destroy(child.gameObject);
        }
        players.Clear();

        foreach (Transform child in statRoot.transform)
        {
            Destroy(child.gameObject);
        }
        statistics.Clear();

        foreach (Transform child in npcRoot.transform)
        {
            Destroy(child.gameObject);
        }
        npcWalker.Clear();

        foreach (Transform child in pointsRoot.transform)
        {
            Destroy(child.gameObject);
        }
        points.Clear();

        Start();
    }
}
