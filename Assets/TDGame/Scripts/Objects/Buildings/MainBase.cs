using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : BuildingBaseClass
{
    public GameObject[] Towers;
    // position to spawn the Towers ( set in inspector)
    public Transform[] towerPos;

    public override void OnEnable()
    {
        //set state
        currentState = State.idle;
        Towers = new GameObject[2];
   
   
        OnEnable_core();
    }

    private void Start()
    {
        // spawn Defensive Towers
        SpawnTowers();
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
    /// Spawn Defensive Towers
    /// </summary>
    void SpawnTowers()
    {
        GameObject TowerPrefab;
        // get the Player specific Prefab
        if (Player == 1)
        {
        
             TowerPrefab = Resources.Load("prefabs/Tower") as GameObject;
        }
        else
        {
            TowerPrefab = Resources.Load("prefabs/Tower 1") as GameObject;

        }

        // Instantiate The Tower
        Towers[0] = Instantiate(TowerPrefab, this.transform);
        //Set position
        Towers[0].transform.localPosition = towerPos[0].localPosition;
        // set rotation
        Towers[0].transform.localRotation = towerPos[0].localRotation;
        Tower T = Towers[0].GetComponent<Tower>();
        // make sure the tower knows what playewr it belongs to
        T.Player = Player;

        Towers[1] = Instantiate(TowerPrefab, this.transform);
        Towers[1].transform.localPosition = towerPos[1].localPosition;
        Towers[1].transform.localRotation = towerPos[1].localRotation;
        T = Towers[1].GetComponent<Tower>();
        T.Player = Player;
    }

    /// <summary>
    /// Handling states;
    /// </summary>
    /// <param name="currentState"></param>
    void handleState(State State)
    {

        if (TDGameManager.Instance.currentState == TDGameManager.gameState.waitingForPlayers)
        {
            return;
        }


        switch (State)
        {
            case State.attacking:

                attack = true;

                break;


            case State.spawning:

             
                spawn(SpawnObject, SpawnPoint, transform.parent);
                currentState = State.idle;
               
                break;



            case State.idle:
           
                if (shouldSpawn)
                {                
                    currentState = State.spawning;
                }

                break;





        }


    }
    /// <summary>
    /// Spawn an Object and set its Target (Very Basic)
    /// </summary>
    /// <param name="Object"></param>
    /// <param name="where"></param>
    /// <param name="Parent"></param>
    public override void spawn(GameObject Object, Transform where, Transform Parent)
    {
        base.spawn(Object, where, Parent);
        if (lastSpawnedObject)
        {
            lastSpawnedObject.transform.parent = this.transform.parent;
            Infantry InfantryScript = lastSpawnedObject.GetComponent<Infantry>();
            InfantryScript.Player = Player;
            /// player Specific Target Logic
            if (Player == 1)
            {

                InfantryScript.Target = TDGameManager.Instance.field.LaneEnd2;
            }
            else
            {
                InfantryScript.Target = TDGameManager.Instance.field.LaneEnd1;
            }
            if (Player == 1)
            {
                TDGameManager.Instance.infantryRound.Add(lastSpawnedObject);
            }
            else
            {
                TDGameManager.Instance.infantryTriangle.Add(lastSpawnedObject);
            }
           
        }

    }
    /// <summary>
    /// take damage
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="byWhom"></param>
    public override void take_damage(int amount, GameObject byWhom)
    {
        foreach (GameObject go in Towers)
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
        
        
        take_damage_core(amount, byWhom);
    }
}
