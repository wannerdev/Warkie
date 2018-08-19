using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BuildingBaseClass : TDBaseClass
{

    public Transform SpawnPoint;
    public bool attack,shouldSpawn;
    public GameObject SpawnObject;
    public GameObject lastSpawnedObject;
    public float SpawnRate;
    public int maxSpawn;
    public  int spawnCount;


    /// <summary>
    /// shared logic here to begin placing a building
    /// </summary>
    protected  virtual void startPlacing_core()
    {
        // disable collider
        col.enabled = false;


    }
    /// <summary>
    /// shared logic here to finalize placement
    /// </summary>
    protected virtual void finalizePlacing_core()
    {

        // enable collider
        col.enabled = true;
      
        if (Player == 1)
        {
            TDGameManager.Instance.buildingsRound.Add(this.gameObject);

        }
        else
        {
            TDGameManager.Instance.buildingsTriangle.Add(this.gameObject);
        }
  

    }
    /// <summary>
    /// this delays spawn accoridng to the spawn rate
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator wait_for_spawn(float time)
    {
        //Debug.Log("this is happening");
        yield return new WaitForSeconds(time);
        shouldSpawn = true ;
    }
    /// <summary>
    /// shared logic for all buildings that spawn things
    /// </summary>
    /// <param name="Object"></param>

    protected virtual void spawn_core(GameObject Object, Transform where, Transform Parent)
    {
        // if I have spawned more than should be spawned, then do nothing
        if (spawnCount >= maxSpawn)
        {

            return;
        }
        // make sure to set spawn trigger to false
        shouldSpawn = false;
        // instantiate
        GameObject go = Instantiate(Object, where.position, Parent.rotation);

        // Logic for navmesh agents ( probably will be removed since we dont use navemsh agents anymore)
        NavMeshAgent agent = go.GetComponent<NavMeshAgent>();
        //set positgion
        if (agent)
        {

            agent.Warp(where.position);

        }
        else
        {
            go.transform.position = where.position;
        }
        // set rotation
        go.transform.rotation = where.rotation;
     
        // cache spawned object ( Maybe change this to a list)
        lastSpawnedObject = go;
        // incrfease spaqwn count
        spawnCount++;
        // start coroutine to delay next spawn
        StartCoroutine(wait_for_spawn(SpawnRate));

    }
  


    /// <summary>
    /// building specific death logic
    /// </summary>
    public override void death()
    {
        // make sure to remove me from any important lists
        if (Player == 1)
        {
            TDGameManager.Instance.buildingsRound.Remove(gameObject);

        }
        else
        {

            TDGameManager.Instance.buildingsTriangle.Remove(gameObject);
        }


        die_core();
    }
    public virtual void spawn(GameObject Object, Transform where, Transform Parent)
    {
        /* will be overridden */
        spawn_core(Object, where, Parent);
    }

    public virtual void startPlacing()
    {

        /* will be overridden */
        startPlacing_core();
    }

    public virtual void finalizePlacing()
    {

        /* will be overridden */
        finalizePlacing_core();
    }

}
