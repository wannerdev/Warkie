using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
    
    public GameObject mainMenu;
    public Text netStatus;
	
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("version01");
    }
    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    void OnJoinedLobby(){
        mainMenu.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    void Update()
    {
        Debug.Log("other players: " + PhotonNetwork.otherPlayers.Length);
    }

    void OnJoinedRoom(){
        netStatus.text = "Player: " + PhotonNetwork.player + " , Room: "+PhotonNetwork.room;
        SceneManager.LoadScene("Main Demo Multiplayer");
    }
}
