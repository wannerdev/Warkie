using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BuildingBaseClass
{
    public ObjectShooter spawner;
    // Use this for initialization
    void Start()
    {
        currentState = State.beingPlaced;
    }

    public override void startPlacing()
    {
        startPlacing_core();
    
        Material spawningMat = Resources.Load("WallMat_transparent") as Material;
        renderer.material = spawningMat;
    }

    public override void finalizePlacing()
    {
        Debug.Log("FINALIZING WALL");
        finalizePlacing_core();
        Material finalMat = Resources.Load("WallMat") as Material;
        renderer.material = finalMat;
    }
    // fixed Update 
    public override void FixedUpdate()
    {



        // if I am dead return
        if (amDead)
        {
            death();
            return;
        }

        FixedUpdate_core();
        // handle States
        handleState(currentState);


    }

    /// <summary>
    /// Handling states;
    /// </summary>
    /// <param name="currentState"></param>
    void handleState(State State)
    {


        switch (State)
        {


            case State.beingPlaced:


             

                break;



            case State.idle:


                break;





        }


    }



}
