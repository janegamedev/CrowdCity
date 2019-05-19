using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Material mat;
    public int amount;

    public float speed;
    
    CharacterController controller;
    Vector3 moveDir = Vector3.zero;
    
    private void Start()
    {
        amount = 1;
        mat.color = Random.ColorHSV();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized;
        moveDir *= speed;

        controller.Move(moveDir * Time.deltaTime);

        if (moveDir!= Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDir);

        foreach (Collider col in Physics.OverlapSphere(transform.position, 2))
        {
            if (col.gameObject.tag=="Npc")
            {
                if (col.GetComponent<NpcController>().isFollowing)
                {
                    //check color and amount
                }
                else
                {
                    col.GetComponent<NpcController>().AddPlayer(gameObject);
                    AddAmount(1);
                }
            }
        } 
    }

    public void AddAmount(int number)
    {
        amount+=number;
    }

}
