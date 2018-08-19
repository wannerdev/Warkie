using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : BuildingBaseClass {
    public Transform Target;
    public float range;
    public GameObject Projectile;
    public Transform projectileOrigin,Turret;
    public float projectileSpeed = 1.0f;
    public float fireRate = 1.0f;
    public Vector3 shotdir;
    bool shouldShoot = true;

    /// <summary>
    ///  Base Class for buildings, has basic functions like calculating directions, shooting, spawning , dying etc
    /// </summary>
    public override void OnEnable()
    {
        currentState = State.idle;
        OnEnable_core();
    }

    /// <summary>
    /// calculate the directional vector which poinnts towards the target
    /// </summary>
    /// <param name="Target"></param>
    void calcDir(Vector3 Target)
    {

        shotdir = Target - Turret.position;

        float distance = shotdir.magnitude;

        shotdir = shotdir / distance;


    }
    /// <summary>
    /// shoot a Projectile
    /// </summary>
    /// <param name="Target"></param>
    public void shoot(Vector3 Target)
    {
       
       // Turret.LookAt(Target);
        SpawnProjectile(projectileOrigin.position, Target);
        shouldShoot = false;
          attack = false;


    }
    /// <summary>
    ///  this waits for " time" amount of seconds, use this to wait inbetween shots 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator wait_for_shot(float time)
    {
       
        yield return new WaitForSeconds(time);
        shouldShoot = true;
    }
    /// <summary>
    /// Spawn teh Projectile which is to be fired
    /// </summary>
    /// <param name="Origin"></param>
    /// <param name="Target"></param>
    public void SpawnProjectile(Vector3 Origin, Vector3 Target)
    {
        // get Direction
        calcDir(Target);


        // instantiate Projectile
        GameObject Shot = Instantiate(Projectile) as GameObject;
        Shot.name = "SHOT TOWER";
        // set Pos
        Shot.transform.position = Origin;
     
        // get shot ( driving) compnoent
        Shot shotScript = Shot.GetComponent<Shot>();
         
        shotScript.dir = shotdir;
        shotScript.speed = projectileSpeed;
        shotScript.Parent = this.gameObject;

        // set physics coillison layers
        if (Player == 1)
        {
            // red team
            Shot.layer = 9;
        }
        else
        {
            // blue
            Shot.layer = 8;
        }
        // start the wait
        StartCoroutine(wait_for_shot(fireRate));
    }

    // fixed Updated function, veryting related spawning shotting etc should be in here
    public override void FixedUpdate()
    {
        // if im dead only die
        if (amDead)
        {
            death();
            return;
        }
        // if im supposed to attack
        if (attack)
        {
            // if im still waoiting inbetween shots return
            if (!shouldShoot)
            {
                return;
            }
            // attack
            if (Target)
            {
                // shoot át target
                shoot(Target.position);

            }
            else
            {
                // I have no target
                Debug.Log("NO VALID TARGET");
                attack = false;
                currentState = State.idle;
            }


        }


      // handle states 
        handleState(currentState);

        FixedUpdate_core();

    }

    /// <summary>
    /// Handling states, satet specific logfic should go here
    /// </summary>
    /// <param name="currentState"></param>
    void handleState(State State)
    {


        switch (State)
        {
            case State.attacking:

                attack = true;

                break;


       



            case State.idle:

          

                break;





        }


    }
    // take damage
    public override void take_damage(int amount, GameObject byWhom)
    {
        // if I dont have a target set the thig that hurt me as my target

        MainBase M = GetComponentInParent<MainBase>();
        if (M)
        {
            foreach (GameObject go in M.Towers)
            {
                if (go)
                {
                    Tower T = go.GetComponent<Tower>();
                    // set target of the towers to the object that is causinng damage to me
                    if (!T.Target)
                    {

                        T.Target = byWhom.transform;
                        if (!T.attack)
                        {
                            T.currentState = State.attacking;
                        }
                    }

                }

            }

        }

  
        take_damage_core(amount, byWhom);

    }
}
