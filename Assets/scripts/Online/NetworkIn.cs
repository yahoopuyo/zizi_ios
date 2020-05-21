using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkIn : MonoBehaviour
{
    ModeData md;
    DrawOnline draw;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PhotonNetwork.player.NickName);
        md = GameObject.Find("ModeData").GetComponent<ModeData>();
        //draw = GetComponent<DrawOnline>();
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)   //抜けたときの挙動
    {
        Debug.Log("on disconnected called");
        String outPlayerName = otherPlayer.NickName;
        int outPlayerNum = Array.IndexOf(md.playerInfo, outPlayerName);
        md.playerInfo[outPlayerNum] = "Com";
        /*
        if (md.numOfPlayer == 2 && outPlayerName == "1") md.playerInfo[2] = "Com";
        if (md.numOfPlayer != 2 && outPlayerName == "1") md.playerInfo[1] = "Com";
        if (outPlayerName == "2") md.playerInfo[2] = "Com";
        if (outPlayerName == "3") md.playerInfo[3] = "Com";
        */
        md.numOfPlayer--;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
