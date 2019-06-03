using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{

    public enum State {Wondering, Following, Dead};

    public State currentState;


    int offset = 3;
    Vector3 dir;
    public NavMeshAgent agent;
    public GameObject player;

    float timer;


    private void Start()
    {
        dir = transform.position;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Wondering: 
                if(agent.velocity == Vector3.zero)
                {
                    dir = transform.position + Random.insideUnitSphere * Random.Range(10, 40);
                }
                break;
            case State.Following:
                dir = player.transform.position;
                CheckCollider();
                break;
        }

        agent.SetDestination(dir);
    }


    public void AddPlayer(GameObject _player)
    {
        if (currentState == State.Wondering)
        {
            GameManager.GM.RemoveNpcFromArray(gameObject);
        }

        currentState = State.Following;
        player = _player;
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", _player.GetComponentInChildren<Renderer>().material.color);
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_OutlineColor", _player.GetComponentInChildren<Renderer>().material.color);
        agent.stoppingDistance = offset;
        agent.speed = 8;
    }

    public void CheckCollider()
    {
        if (timer>=2)
        {
            foreach (Collider col in Physics.OverlapSphere(transform.position, 2))
            {
                if (col.gameObject.tag == "Npc" && col.gameObject.GetComponent<NpcController>().player != player)
                {
                    player.GetComponent<Agent>().AddNpc(col.gameObject);
                    timer = 0;
                }
                else if (col.gameObject.tag == "Player" && col.GetComponent<Agent>().amount < player.GetComponent<Agent>().amount)
                {
                    player.GetComponent<Agent>().KillPlayer(col.gameObject);
                    timer = 0;
                }
            }
        }

        timer += Time.deltaTime;
    }
}
