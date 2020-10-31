using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [Header("Class References")]
    public InputHandler input;
    //public PlayerHealth healthIndicator;

    [Header("Player Settings")]
    public float normalSpeed;
    public float hideSpeed;

    [Header("Material References")]
    public Material playerMat;
    public Material hideMat;



    private void Update()
    {
        Command command = input.handleInput();
        if (command != null && command.GetType() != typeof(MoveCommand))
        {
            command.executePlayer(this);
        }

        //healthIndicator.updateHealth(this);
        if (Input.GetKeyDown(KeyCode.Space))
        {
           this.GetComponent<MeshRenderer>().material = hideMat;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.GetComponent<MeshRenderer>().material = playerMat;
        }
    }


    private void FixedUpdate()
    {
        Command commandPhysics = input.handleMovement();
        if (commandPhysics != null && commandPhysics.GetType() == typeof(MoveCommand))
        {
            commandPhysics.execute(this);
        }


    }
}
