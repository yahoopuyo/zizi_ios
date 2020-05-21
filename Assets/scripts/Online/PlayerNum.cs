using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNum : MonoBehaviour
{
    public int playernum;
    public int nextplayernum;
    public int nextnextplayernum;
    public int nextnextnextplayernum;
    private string[] playerInfo;
    ModeData md;
    TurnManagerOnline tmo;
    Text text0;
    Text text1;
    Text text2;
    Text text3;
    public int playerFontSize;
    	

    // Start is called before the first frame update
    void Start()
    {
        // playernum
	    //playernum = 0;
	    md = GameObject.Find("ModeData").GetComponent<ModeData>();
        tmo = GameObject.Find("GameManager").GetComponent<TurnManagerOnline>();
        playernum = md.player;
	    nextplayernum = (playernum +1) % 4;
	    nextnextplayernum = (playernum + 2) % 4;
	    nextnextnextplayernum = (playernum + 3) % 4;
	    text0 = GameObject.Find("yourPlayerNum").GetComponent<Text>();
	    text1 = GameObject.Find("nextPlayerNum").GetComponent<Text>();
	    text2 = GameObject.Find("nextnextPlayerNum").GetComponent<Text>();
	    text3 = GameObject.Find("nextnextnextPlayerNum").GetComponent<Text>();
        playerFontSize = 50;

        /*
        text0.text = "Player" + playernum.ToString();
        text1.text = "Player" + nextplayernum.ToString();
        text2.text = "Player" + nextnextplayernum.ToString();
        text3.text = "Player" + nextnextnextplayernum.ToString();
        */
    }

        // Update is called once per frame
    void Update()
    {
        playerInfo = md.playerInfo;
        playernum = md.player;
        nextplayernum = (playernum + 1) % 4;
        nextnextplayernum = (playernum + 2) % 4;
        nextnextnextplayernum = (playernum + 3) % 4;
        text0.text = playerInfo[playernum];
        text1.text = playerInfo[nextplayernum];
        text2.text = playerInfo[nextnextplayernum];
        text3.text = playerInfo[nextnextnextplayernum];
        if (text0.text != "Com") text0.fontSize = playerFontSize;
        if (text1.text != "Com") text1.fontSize = playerFontSize;
        if (text2.text != "Com") text2.fontSize = playerFontSize;
        if (text3.text != "Com") text3.fontSize = playerFontSize;
        text0.color = Color.black;
        text1.color = Color.black;
        text2.color = Color.black;
        text3.color = Color.black;
        int tmp = tmo.turnPlayer;
        if (tmp == playernum) text0.color = Color.red;
        if (tmp == nextplayernum) text1.color = Color.red;
        if (tmp == nextnextplayernum) text2.color = Color.red;
        if (tmp == nextnextnextplayernum) text3.color = Color.red;
        /*
        switch (tmo.turnPlayer)
        {
            case playernum:
                text0.color = Color.red;
                break;
            case nextplayernum:
                text1.color = Color.red;
                break;
            case nextnextplayernum:
                text2.color = Color.red;
                break;
            case nextnextnextplayernum:
                text3.color = Color.red;
                break;
        }*/
    }
}
