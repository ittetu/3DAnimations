using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OnlineMaster : MonoBehaviourPunCallBacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }
}
