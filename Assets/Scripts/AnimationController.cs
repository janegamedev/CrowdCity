using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    public AnimationController anim;

    private void Start()
    {
        anim = GetComponent<AnimationController>();
    }

    private void Update()
    {
        if (GetComponent<NavMeshAgent>().velocity == Vector3.zero)
        {
            anim.GetComponent<Animator>().SetBool("isRunning", false);
        }
        else
        {
            anim.GetComponent<Animator>().SetBool("isRunning", true);
        }
    }

    
}
