/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// 
/// Changes made to this file could be overwritten when upgrading the Vuforia version. 
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class CustomTrackableEventHandler : Photon.MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    public bool foundBoard;

    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public Initializer Init;

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        if (!foundBoard)
        {
            if (!Init.enabled)
            {
                Init.enabled = true;
            }
        
           /*TDGameManager.Instance.SpawnBase(PhotonNetwork.player.ID);
           if (TDGameManager.Instance.rdyPlayers < 2)
            {

                TDGameManager.Instance.rdyPlayers++;
            }
      
            if (TDGameManager.Instance.rdyPlayers == 2)
            {
                foundBoard = true;
            }
            
            /*
            if (PhotonNetwork.player.ID == 2)
             {
                GameObject base1 = PhotonNetwork.Instantiate("prefabs/Base 1", spawnPoint1.transform.position, spawnPoint1.transform.rotation, 0);
                this.photonView.RPC("setParentBase", PhotonTargets.All, (object)base1.GetComponent<PhotonView>().viewID);

        }
        if (PhotonNetwork.player.ID == 1)
             {
          

               
                 GameObject base2 = PhotonNetwork.Instantiate("prefabs/Base", spawnPoint2.transform.position, spawnPoint2.transform.rotation, 0);
                 this.photonView.RPC("setParentBase", PhotonTargets.All, (object)base2.GetComponent<PhotonView>().viewID);
                 
            }*/

        }
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }

    [PunRPC]
    void startGame()
    {
        Debug.Log("trying to start");
        TDGameManager.Instance.GameStart();
        /*
        if (PhotonNetwork.player.ID ==1)
        {
            Debug.Log("trying to start 2");
            TDGameManager.Instance.GameStart();
        }
        else
        {
       
        }*/
    }

    [PunRPC]
    void setParentBase(object viewId){
        
        if(PhotonView.Find((int)viewId).transform.name == "Base 1(Clone)"){
            PhotonView.Find((int)viewId).transform.parent = spawnPoint1.transform;
        }else{
            PhotonView.Find((int)viewId).transform.parent = spawnPoint2.transform;
        }
    }
    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

    }

    #endregion // PROTECTED_METHODS
}
