using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public bool isFollowing;  //Check if npc is following player or just wondering

    int offset = 3;
    Vector3 dir;
    public NavMeshAgent agent;
    public GameObject player;
    float lifeTime=30;

    private void Start()
    {
        dir = transform.position;
        SetDir();
    }

    private void Update()
    {
        agent.SetDestination(dir);
        SetDir();

        if(player!= null)
        CheckCollider();
        Timer();
    }

    void SetDir()
    {
        if (!isFollowing && GetComponent<NavMeshAgent>().velocity == Vector3.zero)
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
        dir = transform.position + Random.insideUnitSphere * Random.Range(10,40);
    }

    public void AddPlayer(GameObject _player)
    {
        isFollowing = true;
        player = _player;
        gameObject.GetComponentInChildren<Renderer>().material.color = _player.GetComponent<Renderer>().material.color;
        agent.stoppingDistance = offset;
        agent.speed = 6;
    }

    public void CheckCollider()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, 2))
        {
            if (col.gameObject.tag == "Npc")
            {
                if (col.GetComponent<NpcController>().isFollowing)
                {
                    if (col.GetComponent<NpcController>().player.GetComponent<PlayerMovement>().amount < player.GetComponent<PlayerMovement>().amount)
                    {
                        col.GetComponent<NpcController>().AddPlayer(player);
                        col.GetComponent<NpcController>().player.GetComponent<PlayerMovement>().AddAmount(-1);
                        player.GetComponent<PlayerMovement>().AddAmount(1);
                    }
                }
                else
                {
                    col.GetComponent<NpcController>().AddPlayer(player);
                    player.GetComponent<PlayerMovement>().AddAmount(1);
                }
            }
        }
    }

    void Timer()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0&&!isFollowing)
        {
            Destroy(gameObject);
        }
    }

}
