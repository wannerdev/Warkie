using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TDGameManager : ScriptableObject
{


    public ObjectShooter m_ObjectShooter;
    public GameObject m_LevelGeometry;
    //public string _roundBase, _triangleBase;
     GameObject _roundBase, _triangleBase;
    public gameState currentState ;
    public playingField field;
    private static TDGameManager _instance;
    public int rdyPlayers = 0; 
    public GameObject mainBaseRound, mainBaseTriangle;
    public NavMeshSurface navSurface;
    // lists of allthe important shit
    public List<GameObject> buildingsRound, buildingsTriangle, infantryRound, infantryTriangle;
    public Initializer initializer;
    // jsut some possible gamestates, can be updated and cleaned up
    public enum gameState
    {
        fixingPlane,
        waitingForPlayers,
        playing,
        paused,
        gameOver
    }

    /// <summary>
    /// Instance stuff
    /// </summary>
    public static TDGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Creating Instance of TDGM");
                _instance = (TDGameManager)ScriptableObject.CreateInstance("TDGameManager");

            }
            return _instance;

        }

    }


    // Use this for initialization
    public void Init()
    {
        Debug.Log("Initializing GM");
        // load in the relevant GameObject prefabs
         _triangleBase = Resources.Load("prefabs/Base 1") as GameObject;
          _roundBase = Resources.Load("prefabs/Base") as GameObject;
         //_triangleBase = "prefabs/Base 1";
        // _roundBase = "prefabs/Base";
        infantryTriangle = new List<GameObject>();
        infantryRound = new List<GameObject>();
        buildingsRound = new List<GameObject>();
        buildingsTriangle = new List<GameObject>();

        currentState = gameState.waitingForPlayers;




    }



    // use this to instantiate bases, starting soldiers and start the game
    public void GameStart()
    {
        Debug.Log("GameStarted");
        // instantiate the Bases
        currentState = gameState.playing;
    }

    public void setUpBase(int Player)
    {

        if (Player == 1)
        {
            Debug.Log("Setting Up Base Round");
            mainBaseRound.GetComponent<BuildingBaseClass>().Player = 1;
            // set the parent
            mainBaseRound.transform.parent = field.transform.parent;
            mainBaseRound.name = "mainBaseRound";
            // copy the transform from the spawn location

            mainBaseRound.transform.position = field.BaseSpawnRound.transform.position;
            // set the roatation
            mainBaseRound.transform.localRotation = field.BaseSpawnRound.transform.localRotation;
            // make sure it knows what player it is
            mainBaseRound.GetComponent<BuildingBaseClass>().Player = 1;
            // make sure it is tracked
            buildingsRound.Add(mainBaseRound);
        }
        else
        {
            Debug.Log("Setting Up Base Trianagle");
            mainBaseTriangle.GetComponent<BuildingBaseClass>().Player = 2;
            mainBaseTriangle.transform.parent = field.transform.parent;
            mainBaseTriangle.name = "mainBaseTriangle";

            mainBaseTriangle.transform.position = field.BaseSpawnTriangle.transform.position;
            mainBaseTriangle.transform.localRotation = field.BaseSpawnTriangle.transform.localRotation;
            buildingsRound.Add(mainBaseTriangle);
        }

        if (PhotonNetwork.player.ID == 2)
        {
            initializer.photonView.RPC("startGame", PhotonTargets.All);

        }

    }

    public void SpawnBase(int Player)
    {
    
        if (Player== 1){
            Debug.Log("Spawned Base Round");
            mainBaseRound = Instantiate(_roundBase) as GameObject;
            // mainBaseRound = PhotonNetwork.InstantiateSceneObject(_roundBase, field.BaseSpawnRound.transform.position, Quaternion.identity, 0,null);

            setUpBase(1);

        }
        else
        {

            Debug.Log("Spawned Base Triangle");
            mainBaseTriangle = Instantiate(_triangleBase) as GameObject;
            //mainBaseTriangle = PhotonNetwork.InstantiateSceneObject(_triangleBase, field.BaseSpawnRound.transform.position, Quaternion.identity, 0,null); ;
            setUpBase(2);
        }






    }

    /// <summary>
    /// Copy the transform from a Source Transform to a target Transform ( not sure if we still need this)
    /// </summary>
    /// <param name="Source"></param>
    /// <param name="copyTo"></param>
    /// <param name="local"></param>
    void copyTransform(Transform Source, Transform copyTo, bool local)
    {

        if (local)
        {
            copyTo.localPosition = Source.localPosition;
            copyTo.localRotation = Source.localRotation;

        }
        else
        {
            copyTo.position = Source.position;
            copyTo.rotation = Source.rotation;
        }


        copyTo.localScale = Source.localScale;


    }
}
