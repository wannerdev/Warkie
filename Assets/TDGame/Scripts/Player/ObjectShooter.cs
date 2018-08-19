using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ShotPrefab;

    [SerializeField]
    private GameObject[] m_WallPrefab;

    [SerializeField]
    private GameObject[] m_DefensivePrefab;

    [SerializeField]
    private float m_Force = 5000f;

    public float minimumYValue = 0f;
    public enum fireMode { single, all, placing, defensive, passive }
    public fireMode mode;
    public bool m_WasFireRequested = false;
    private Vector2 m_ScreenPosition;
    public int damage = 10;
    public int Player = 1;
    private GameObject currentWall, currentBarrier;
    public bool placing;
    public LayerMask targetMask;
    public float wallheight = 0.15f;
    public float Barrierheight = 0.05f;

    // request fire 
    public void RequestFire()
    {
        m_WasFireRequested = true;

    }

    // request fire 
    public void RequestPlacing()
    {
        m_WasFireRequested = true;
        placing = true;
        if (mode == fireMode.defensive)
        {
            if (Player == 1)
            {
                targetMask = targetMask | 1 << 12;
            }
            else
            {
                targetMask = targetMask | 1 << 11;
            }

        }

    }

    public void finalizePlacing()
    {

        placing = false;
        if (mode == fireMode.defensive)
        {
      
            createBarrier();
            Barrier B = currentBarrier.GetComponent<Barrier>();
            B.finalizePlacing();
            currentBarrier = null;
            if (Player == 1)
            {
                targetMask &= ~(1 << 12);
            }
            else
            {
                targetMask &= ~(1 << 11);
            }
        }
        else if (mode == fireMode.placing)
        {
            createWall();


        }



    }
    /// <summary>
    /// this fires a single shot from the center of the screen
    /// </summary>
    void singleShot()
    {
        Debug.Log("!!!!!");
        // find the center of the screen & cast a ray from it
        var ray = Camera.main.ScreenPointToRay(m_ScreenPosition);
        var go = Instantiate(m_ShotPrefab, ray.origin, Quaternion.identity); //  + ray.direction * 2f, Quaternion.identity);  keep this here pls 
        var rigidbody = go.GetComponent<Rigidbody>();
        PlayerShot P = go.GetComponent<PlayerShot>();
        // set damage & other important vars
        P.damage = damage;
        P.Parent = this.gameObject;
        P.Player = Player;

        // if the shot has a rigidbody
        if (rigidbody != null)
        {
            // apply force to it
            var force = ray.direction * m_Force;
            rigidbody.AddForce(force);
        }

        /* var remover = go.GetComponent<RemoveRigidbody>();
         remover.minYPosition = minimumYValue;*/

    }


    /// <summary>
    /// this fires a single shot from the center of the screen
    /// </summary>
    void createWall()
    {

        // find the center of the screen & cast a ray from it
        var ray = Camera.main.ScreenPointToRay(m_ScreenPosition);
        RaycastHit hit;
        bool didHit = Physics.Raycast(ray.origin, ray.direction, out hit, 5, targetMask);

        if (didHit)
        {

            Wall W;
            // wallheight = 0.15f;
            Vector3 Wallpos = hit.point + hit.collider.transform.up * wallheight;
            if (!currentWall)
            {


                if (Player == 1)
                {
                    currentWall = Instantiate(m_WallPrefab[0], Wallpos, hit.collider.transform.rotation); //  + ray.direction * 2f, Quaternion.identity);  keep this here pls 

                }
                else
                {

                    currentWall =Instantiate(m_WallPrefab[1], Wallpos, hit.collider.transform.rotation); //  + ray.direction * 2f, Quaternion.identity);  keep this here pls 

                }
                W = currentWall.GetComponent<Wall>();
                W.Player = Player;
                W.startPlacing();
                W.spawner = this;

            }
            else
            {

                W = currentWall.GetComponent<Wall>();

                if (W)
                {
                    if (!placing)
                    {
                        //    W.finalizePlacing();
                       // Wall W = currentWall.GetComponent<Wall>();
                    

                        W.finalizePlacing();
                        currentWall = null;
                    }
                    else
                    {

                        W.transform.position = Wallpos;
                    }

                }
            }

        }





    }

    /// <summary>
    /// this fires a single shot from the center of the screen
    /// </summary>
    void createBarrier()
    {

        // find the center of the screen & cast a ray from it
        var ray = Camera.main.ScreenPointToRay(m_ScreenPosition);
        RaycastHit hit;
        bool didHit = Physics.Raycast(ray.origin, ray.direction, out hit, 5, targetMask);

        if (didHit)
        {
            Barrier B;
            // wallheight = 0.15f;
            Vector3 Wallpos = hit.point + hit.collider.transform.up * Barrierheight;
            if (!currentBarrier)
            {


                if (Player == 1)
                {
                    currentBarrier = Instantiate(m_DefensivePrefab[0], Wallpos, hit.collider.transform.rotation); //  + ray.direction * 2f, Quaternion.identity);  keep this here pls 

                }
                else
                {

                    currentBarrier = Instantiate(m_DefensivePrefab[0], Wallpos, hit.collider.transform.rotation); //  + ray.direction * 2f, Quaternion.identity);  keep this here pls 

                }
                B = currentBarrier.GetComponent<Barrier>();
                B.Player = Player;
                B.startPlacing();
                //B.spawner = this;

            }
            else
            {

                B = currentBarrier.GetComponent<Barrier>();

                if (B)
                {
                    if (!placing)
                    {
                      //  B.finalizePlacing();
                  
                    }
                    else
                    {
           
                        B.transform.position = Wallpos;
                    }

                }
            }

        }
    }

    /// <summary>
    ///  this attack effects all enemy infantry atm, this should be changed to a radius maybe or only a select few
    /// </summary>
    void areaShot()
    {

        // Iterate through the enemies and do damage 
        Debug.Log("!!!!!!");
        if (Player == 1)
        {
            foreach (GameObject G in TDGameManager.Instance.infantryTriangle)
            {
                if (G)
                {
                    TDBaseClass B = G.GetComponent<TDBaseClass>();
                    if (B)
                    {

                        B.take_damage(damage / 5, this.gameObject);
                    }
                }


            }


        }
        else
        {
            foreach (GameObject G in TDGameManager.Instance.infantryRound)
            {
                if (G)
                {
                    TDBaseClass B = G.GetComponent<TDBaseClass>();
                    if (B)
                    {

                        B.take_damage(damage / 5, this.gameObject);
                    }
                }


            }

        }

    }
    /// <summary>
    /// switch firing modes
    /// </summary>
    public void switchMode()
    {


        if (mode == fireMode.single)
        {
            mode = fireMode.all;
        }
        else if (mode == fireMode.all)
        {
            mode = fireMode.placing;
        }
        else if (mode == fireMode.placing)
        {
            mode = fireMode.defensive;
        }
        else if (mode == fireMode.defensive)
        {
            mode = fireMode.single;
        }
    }

    void Update()
    {
        // fire logic
        if (m_WasFireRequested || placing)
        {
            m_ScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);
            if (!placing)
            {
                if (mode == fireMode.single)
                {
                    singleShot();
                }
                else if (mode == fireMode.all)
                {
                    areaShot();
                }
            }
            else
            {

                if (mode == fireMode.placing)
                {
                    createWall();
                }
                else if (mode == fireMode.defensive)
                {
                    createBarrier();
                }

            }

            m_WasFireRequested = false;
        }

    }
}
