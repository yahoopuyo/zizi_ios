using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System.Globalization;
using System;
public class ZiziDeck : UnityEngine.MonoBehaviour
{
    private List<int> cards;
    private int zizi;
    private int seed;
    public bool shared=false;
    ModeData md;
    public List<int> GetCards()
    {
        return cards;
    }

    public int GetZizi()
    {
        return zizi;
    }

    public void Shuffle()
    {
        //UnityEngine.Random.InitState(seed);
        if (cards == null)
        {
            cards = new List<int>();
        }
        else
        {
            cards.Clear();
        }

        for (int i = 0; i < 52; i++)
        {
            cards.Add(i);
        }
        int n = cards.Count;
        while (n > 0)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            int tmp = cards[k];
            cards[k] = cards[n];
            cards[n] = tmp;

        }
        zizi = cards[51];
        cards.RemoveAt(51);
    }

    public static void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        while (n > 0)
        {
            n--;
            int k = UnityEngine.Random.Range(0,n+1);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    void ShufflePlayers(int numOfPlayer)
    {
        Debug.Log("shuffle players called");
        string[] playerInfo = new string[4];
        for (int i = 0; i < 4; i++)
        {
            if (i < numOfPlayer) playerInfo[i] = PhotonNetwork.playerList[i].NickName;
            else playerInfo[i] = "Com";
        }
        if (numOfPlayer == 2)//players must be set diag 
        {
            int k = UnityEngine.Random.Range(0, 2);
            playerInfo[3-k] = playerInfo[k];
            playerInfo[k] = "Com";
        }
        else ShuffleArray<string>(playerInfo);
        md.playerInfo = playerInfo;
        
    }
    [PunRPC]
    void SendSeed(int num,int numOfPlayer,string[] playerInfo)
    {
        md.numOfPlayer = numOfPlayer;
        UnityEngine.Random.InitState(num);
        Shuffle();
        shared = true;
        //if (numOfPlayer == 2 && md.player == 1) md.player = 2;//仮の処理
        md.playerInfo = playerInfo;
        md.player = Array.IndexOf(playerInfo, PhotonNetwork.playerName);
        md.UpdateScore();
    }
    
    void Start()
    {
        Debug.Log("zizideck called");
        md = GameObject.Find("ModeData").GetComponent<ModeData>();
        if (!md.IsSolo() && md.IsHost())
        {
            seed = UnityEngine.Random.Range(0, 10000);
            PhotonView view = GetComponent<PhotonView>();
            ShufflePlayers(md.numOfPlayer);
            view.RPC("SendSeed", PhotonTargets.All, seed,md.numOfPlayer,md.playerInfo);
        }
        else if (md.IsSolo()) Shuffle();

    }

    
}
