using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float timeLeft=3;

    public GameObject npc;

    private void Start()
    {
        SpawnNpc();
    }

    private void Update()
    {
        Timer();
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
        Instantiate(npc);
    }
  
}
