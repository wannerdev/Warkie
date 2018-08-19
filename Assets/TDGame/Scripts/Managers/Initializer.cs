using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

using UnityEngine.AI;

/// <summary>
/// This class kick all of the shít off and basically is needed to carry out the TDGameManager INstance. This class is activated by teh vuforia plane finder
/// </summary>
public class Initializer : Photon.MonoBehaviour
{

    [SerializeField]
    public ObjectShooter m_ObjectShooter;

    [SerializeField]
    public GameObject m_LevelGeometry;
    [SerializeField]
    public playingField field;
    [SerializeField]


    public bool go;
    bool one;

    // Use this for initialization
    public void Start()
    {
        // Initialize the TD GameManager
        TDGameManager.Instance.Init();
        // copy teh important vars
        copyVars();
        // start the game
         TDGameManager.Instance.SpawnBase(1);
         TDGameManager.Instance.SpawnBase(2);
   
   
        // make sure our GUI knows that we are ztransitioning to the game Start

    }

    private void Update()
    {
        if (go && !one || PhotonNetwork.countOfPlayers == 2 && TDGameManager.Instance.rdyPlayers ==2 && !one)
        {

            spawnBases();
           // this.photonView.RPC("spawnBases", PhotonTargets.All);
            GetComponent<TD_GUI>().foundFloor = true;
            one = true;
          
        }
    }
    /// <summary>
    /// pass relevant Scene Variables to the GameManager
    /// </summary>
    void copyVars()
    {
        Debug.Log("Copied the Vars");

        TDGameManager.Instance.m_ObjectShooter = m_ObjectShooter;

        TDGameManager.Instance.m_LevelGeometry = m_LevelGeometry;
        TDGameManager.Instance.field = field;
        TDGameManager.Instance.initializer = this;
        //  TDGameManager.Instance.navSurface = navSurface;




    }


    [PunRPC]
    public void startGame()
    {
        TDGameManager.Instance.GameStart();
    }


    [PunRPC]

    void SetBases()
    {

        TDGameManager.Instance.setUpBase(1);
        TDGameManager.Instance.setUpBase(2);
        //TDGameManager.Instance.GameStart();

    }
    [PunRPC]
    void spawnBases()
    {

        TDGameManager.Instance.SpawnBase(1);
        TDGameManager.Instance.SpawnBase(2);
        this.photonView.RPC("SetBases", PhotonTargets.All);
        //  SetBases();
    }




}
