using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public bool isFollowing;  //Check if npc is following player or just wondering

    Vector3 dir;
    public NavMeshAgent agent;
    GameObject player;

    private void Start()
    {
        dir = transform.position;
        SetDir();
    }

    private void Update()
    {
        agent.SetDestination(dir);

        SetDir();
    }

    void SetDir()
    {
        if (!isFollowing&&transform.position==dir)
        {
            GenerateDir();
        }
        else if(isFollowing)
        {
            dir = player.transform.position;
        }
    }

    void GenerateDir()
    {
        dir = new Vector3(Random.Range(-50, 50), .5166669f, Random.Range(-50, 50));

        Ray ray = new Ray(new Vector3(dir.x,10,dir.z), dir);
        RaycastHit hit;

        if(!Physics.Raycast(ray, out hit))
        {
            GenerateDir();
        }
        Debug.Log(dir);
    }

    public void AddPlayer(GameObject _player)
    {
        isFollowing = true;
        player = _player;
        gameObject.GetComponent<Renderer>().material.color = _player.GetComponent<Renderer>().material.color;
        agent.stoppingDistance = 2+_player.GetComponent<PlayerMovement>().amount;
    }

}
