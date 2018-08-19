using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : BuildingBaseClass
{
    //public ObjectShooter spawner;
    internal float startAlpha;
    public float lifeTime = 15.0f;
    // Use this for initialization
    void Start()
    {
        currentState = State.beingPlaced;
        startAlpha = renderer.material.color.a;
        amInvulnerable = true;
        
    }

    public override void startPlacing()
    {
        startPlacing_core();

    
      
         Color col = renderer.material.color ;
         col.a /= 2;
        renderer.material.color = col;
  
    }

    public override void finalizePlacing()
    {
        Debug.Log("FINALIZING Barrier");
        finalizePlacing_core();

        Color col = renderer.material.color;
        col.a =startAlpha;
        renderer.material.color = col;
        Destroy(gameObject, lifeTime);
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
