using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics;

public class ModeData : MonoBehaviour
{
    GameObject menu;
    GameObject mode;
    GameObject cpu;
    GameObject inst;
    private bool Easy;
    private bool Solo;
    public bool isHost;
    public int computerLevel;
    public int player;
    public int numOfPlayer;
    public string roomName;
    public List<int> score;
    public string[] playerInfo;
    public string[] prePlayerInfo;
    public List<int> comIndex = new List<int>();
    public List<int> preScore;
    // Start is called before the first frame update

    void Start()
    {
        DontDestroyOnLoad(this);
        mode = GameObject.Find("ModeSelectForSoloPanel"); 
        mode.SetActive(false);
        cpu = GameObject.Find("computerLevelForSoloPanel");
        cpu.SetActive(false);
        menu = GameObject.Find("MenuPanel");
        menu.SetActive(true);
        inst = GameObject.Find("InstructionPanel");
        inst.SetActive(false);
        score = new List<int>() { 0,0,0,0 };
        playerInfo = new string[4] { "player", "Com", "Com", "Com" }; //for solo play
        prePlayerInfo = new string[4]; 
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnReloadGame()
    {
        Array.Copy(playerInfo,prePlayerInfo,4);
    }
    public void UpdateScore()
    {
        UnityEngine.Debug.Log("update score called");
        if (prePlayerInfo[0] == null) return; //1回目なら特にすることなし
        preScore = new List<int>(score);
        score = new List<int> { -1, -1, -1, -1 };
        comIndex = new List<int> { 0, 1, 2, 3 };

        for (int i = 0; i < 4; i++)
        {
            string playeri = playerInfo[i];
            if (playeri != "Com")//playerだけ
            {
                int tmp = Array.IndexOf(prePlayerInfo,playeri);
                UnityEngine.Debug.Log("preplayerinfo.playeri = " + tmp);
                comIndex.Remove(tmp);
                score[i] = preScore[tmp];
            }
        }
        int tmp2 = 0;
        for (int i = 0; i < 4; i++)
        {
            if(score[i] == -1)
            {
                score[i] = preScore[comIndex[tmp2]];
                tmp2++;
            }
        }
    }
    public void OnClickNormal()
    {
        Easy = false;
        mode.SetActive(false);
        cpu.SetActive(true);
    }

    public void OnClickEasy()
    {
        Easy = true;
        mode.SetActive(false);
        cpu.SetActive(true);
    }

    public void OnClickSolo()
    {
        Solo = true;
    }

    public void OnClickOnline()
    {
        Solo = false;
        SceneManager.LoadScene("photontest1");
    }

    public void OnClickStart()
    {
        if (computerLevel != 0) SceneManager.LoadScene("zizitest");
    }

    public bool IsSolo()
    {
        return Solo;
    }
    public bool IsEasy()
    {
        return Easy;
    }
    public bool IsHost()
    {
        return isHost;
    }
}
