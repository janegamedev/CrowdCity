using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float timeLeft=60;

    private void Update()
    {
        Timer();
    }

    void Timer()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft<=0)
            Debug.Log("game over");

        Debug.Log(timeLeft);
    }
}
