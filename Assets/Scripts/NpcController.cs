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
    float lifeTime;
    float timer;


    private void Start()
    {
        lifeTime = Random.Range(20, 40);
        dir = transform.position;
        SetDir();
    }

    private void Update()
    {
        agent.SetDestination(dir);
        SetDir();

        if (isFollowing && timer > 1)
        {
            CheckCollider();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }


        Timer();
    }

    void SetDir()
    {
        if (!isFollowing && agent.velocity == Vector3.zero)
        {
            dir = transform.position + Random.insideUnitSphere * Random.Range(10, 40);
        }
        else if (isFollowing)
        {
            dir = player.transform.position;
        }
    }

    public void AddPlayer(GameObject _player)
    {
        GameManager.GM.RemoveNpcFromArray(gameObject);
        isFollowing = true;
        player = _player;
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", _player.GetComponentInChildren<Renderer>().material.color);
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_OutlineColor", _player.GetComponentInChildren<Renderer>().material.color);
        agent.stoppingDistance = offset;
        agent.speed = 8;
    }

    public void CheckCollider()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, 2))
        {
            if (col.gameObject.tag == "Npc" && col.gameObject.GetComponent<NpcController>().player != player)
            {
                player.GetComponent<Agent>().AddNpc(col.gameObject);
            }
            else if (col.gameObject.tag == "Player" && col.GetComponent<Agent>().amount < player.GetComponent<Agent>().amount)
            {
                player.GetComponent<Agent>().KillPlayer(col.gameObject);
            }
        }
    }

    void Timer()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0 && !isFollowing)
        {
            GameManager.GM.RemoveNpcFromArray(gameObject);
            Destroy(gameObject);
        }
    }
}
