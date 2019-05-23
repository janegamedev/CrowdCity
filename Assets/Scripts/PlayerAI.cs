using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    public int amount;
    public float speed;

    CharacterController controller;
    Vector3 dir = Vector3.zero;

    public NavMeshAgent agent;

    private void Start()
    {
        amount = 1;
        gameObject.GetComponentInChildren<Renderer>().material.color = Random.ColorHSV();
    }

    private void Update()
    {
        agent.SetDestination(dir);
        SetDir();
        CheckCollider();
    }

    void SetDir()
    {
        if (agent.velocity == Vector3.zero)
        {
            GenerateDir();
        }
    }

    void GenerateDir()
    {
        dir = transform.position + Random.insideUnitSphere * Random.Range(10, 40);
    }

    public void AddAmount(int number)
    {
        amount += number;
    }

    public void CheckCollider()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, 2))
        {
            if (col.gameObject.tag == "Npc")
            {
                if (col.GetComponent<NpcController>().isFollowing && col.GetComponent<NpcController>().player != gameObject)
                {
                    if (col.GetComponent<NpcController>().player.GetComponent<PlayerMovement>().amount < amount)
                    {
                        col.GetComponent<NpcController>().AddPlayer(gameObject);
                        col.GetComponent<NpcController>().player.GetComponent<PlayerMovement>().AddAmount(-1);
                        AddAmount(1);
                    }
                    else
                    {
                        //delete one npc from yourself
                    }
                }
                else
                {
                    col.GetComponent<NpcController>().AddPlayer(gameObject);
                    AddAmount(1);
                }
            }
        }
    }
}
