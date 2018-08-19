using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TDBaseClass : MonoBehaviour
{
    TDGameManager tDGameManager;
    [SerializeField]
    public int Player;
    [SerializeField]
    private Transform HealthBar;
    public int hitPoints;
    public int currentHitPoints;
    public bool amDead;
    public bool amInvulnerable;
    public State currentState;
    private Vector3 original_healthbar_scale = new Vector3();
    [HideInInspector]
    public Collider col;
    [HideInInspector]
    public MeshRenderer renderer;

    /// <summary>
    /// These States define specific behaviour for each Object
    /// </summary>
    public enum State
    {
        beingPlaced,
        spawning,
        idle,
        attacking,
        dead,
        moving
        
    }
    /// <summary>
    /// Common init behavior
    /// </summary>
    protected virtual void OnEnable_core()
    {
        // Get the GameManager
        tDGameManager = TDGameManager.Instance;
        // Set healthbar Scale
        if (HealthBar)
        {
            original_healthbar_scale = HealthBar.localScale;
        }

        // set Health ( hitPoints)
        currentHitPoints = hitPoints;
        col = GetComponent<Collider>();
        renderer = GetComponent<MeshRenderer>();
    }
    /// <summary>
    /// Common fixed-update behavior
    /// </summary>
    protected virtual void FixedUpdate_core()
    {
        // if I am dead dont do anything
      if (amDead)
        {
      
            return;
        }

      // if there is a Healthbar Object update it
      if (HealthBar)
        {

            update_healthbar();
        }
    }

    /* Take damage */
    protected virtual void take_damage_core(int amount, GameObject byWhom)
    {

        if (amInvulnerable)
        {
            return;
        }
        // deduct damage from hitPoints
        currentHitPoints -= amount;
        // if there are less or equal zero im dead
        if (currentHitPoints <= 0)
        {

            amDead = true;
            currentState = State.dead;
        }

    }

    /// <summary>
    /// Show the health bar if the enemy took a hit (and if it has one)
    /// </summary>
    protected virtual void update_healthbar()
    {
        HealthBar.localScale = new Vector3(((float)currentHitPoints / (float)hitPoints)* original_healthbar_scale.x, original_healthbar_scale.y, original_healthbar_scale.z);
    }



    /* Shared behavior when dead */
    protected virtual void die_core()
    {
        Destroy(gameObject);

    }


    public virtual void death()
    {
        /* do stuff when I die */
        die_core();
    }


    public virtual void take_damage(int amount, GameObject byWhom)
    {
        /* will be overridden */
        take_damage_core(amount, byWhom);
    }

    public virtual void FixedUpdate()
    {
        /* will be overridden */
        FixedUpdate_core();
    }

    public virtual void OnEnable()
    {
        /* will be overridden */
        OnEnable_core();

    }



}
