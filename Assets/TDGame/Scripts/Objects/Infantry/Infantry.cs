using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Infantry : TDBaseClass
{


    public Transform Target;
    Vector3 TargetPos;
    public float range;
    public GameObject Projectile;
    public Transform projectileOrigin;
    public float projectileSpeed = 1.0f;
    Animator myAnim;
    public float speed = 0.5f;
    public float dist;
    Vector3 Rand, offset;
    public enum Action
    {
        move, attack, idle
    }



    public Action arrivalAction;
    bool attack, arrived, facingTarget;
    public Vector3 shotdir;


    // Use this for initialization
    protected virtual void InfantryStartCore()
    {

        // cache all important variables here
        arrived = true;
     
        myAnim = GetComponentInChildren<Animator>();
        // set the current state
        currentState = State.moving;
        // Some math to create an additional offset for path targets
         Rand = Random.insideUnitSphere * 0.2f;
         offset = Vector3.zero;
        // set the Target Position
        if (Target)
        {

            TargetPos = Target.position + new Vector3(Rand.x, 0, Rand.z) + offset;
        }
   
 
   
    }
    /// <summary>
    /// Vector math to Calculate the direction of the shot
    /// </summary>
    /// <param name="Target"></param>
    void calcDir(Vector3 Target)
    {
       
        shotdir = Target - transform.position;

        float distance = shotdir.magnitude;

        shotdir = shotdir / distance;


    }

    // Update is called once per frame, PUT EVRYTHING RELATED TO PHYSICS AND MOVING IN HERE!!
    public override void FixedUpdate()
    {
        if (Target)
        {
            // set the Target Position
            TargetPos = Target.position + new Vector3(Rand.x, 0, Rand.z) + offset;
        }
        // if Im dead to do anything
        if (amDead)
        {
            death();
            return;
        }
        // if im supposed to attack
        if (attack)
        {
            // if there is a target
            if (Target)
            {
                // set Target
                Target = findTarget();
                // shoot
                shoot(Target.position);

            }
            else
            {
                // I have no target
                Debug.Log("NO VALID TARGET");
                attack = false;
                // go back to idle
                currentState = State.idle;
            }


        }
        // calculated the distanvce between object and Target
         dist = Vector3.Distance(transform.position, Target.position);
         // if I am in Range
      if (dist <= range && !arrived)
        {
   
            // we have arrived at destination
            actionOnArrival(arrivalAction);
         
            arrived = true;
        }
        
      // handle state logic
        handleState(currentState);
        // handle movement related stuff
        FixedUpdate_core();

    }

    /// <summary>
    /// State specific logic goés here
    /// </summary>
    /// <param name="State"></param>
    protected virtual void handleState(State State)
    {


        switch (State)
        {
            case State.attacking:

                attack = true;

                break;


            case State.idle:



                break;



            case State.moving:


                move();
                break;


        }


    }
    /// <summary>
    /// Do an action on arrival, defined here
    /// </summary>
    /// <param name="action"></param>
    protected virtual void actionOnArrival(Action action)
    {
        if (action == Action.attack)
        {
           
            currentState = State.attacking;
       
            attack = true;

        }


    }
    // return closest avaidable Target ( hard coded atm, should find nearest enemy structure and shoot at it)
    Transform findTarget()
    {
        Transform Target;

        if (Player == 1)
        {

            Target = TDGameManager.Instance.mainBaseTriangle.transform;

        }
        else
        {

            Target = TDGameManager.Instance.mainBaseRound.transform;
        }

        return Target;

    }
    /// <summary>
    /// shoot some shit
    /// </summary>
    /// <param name="Target"></param>
    public void shoot(Vector3 Target)
    {
        // make sure im looking at the taregt
        transform.LookAt(Target);
        // trigger animator ( animation triggers the spawn projectile method)
        myAnim.SetTrigger("shoot");
        attack = false;


    }
    /// <summary>
    /// move to a specific point
    /// </summary>
    void move()
    {

        float step = speed * Time.deltaTime;
        currentState = State.moving;
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, step);
  
        arrived = false;


    }
    /// <summary>
    /// spawn a Projectile 
    /// </summary>
    /// <param name="Origin"></param>
    /// <param name="Target"></param>
    public void SpawnProjectile(Vector3 Origin, Vector3 Target)
    {
        // get direction
        calcDir(Target);

        // instantiate Projectile
        GameObject shot =Instantiate(Projectile); 

        // set position
        shot.transform.position = Origin;
        //set parent
        shot.transform.parent = this.transform.parent;

        // get the driving componen t of the shot
        Shot shotScript = shot.GetComponent<Shot>();
        // set direction
        shotScript.dir = shotdir;
        //set speed
        shotScript.speed = projectileSpeed;
        shotScript.Parent = this.gameObject;

        // set physics collision layer
        if (Player == 1)
        {
            // red Team
            shot.layer = 9;
        }
        else
        {
            // blue team
            shot.layer = 8;
        }


    }
    /// <summary>
    /// will be overriden
    /// </summary>
    public override void death()
    {
        // make sure to remove me from any important lists
        if (Player == 1)
        {
            TDGameManager.Instance.infantryRound.Remove(gameObject);

        }
        else
        {

            TDGameManager.Instance.infantryTriangle.Remove(gameObject);
        }


        base.death();
    }
    /// <summary>
    /// will be overriden
    /// </summary>
    public virtual void Start()
    {
        InfantryStartCore();

    }
}
