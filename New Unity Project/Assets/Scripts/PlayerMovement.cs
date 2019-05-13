using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    
    CharacterController controller;
    Vector3 moveDir = Vector3.zero;
    
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized;
        moveDir *= speed;

        controller.Move(moveDir * Time.deltaTime);

        if (moveDir!= Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDir);
   
    }

}
