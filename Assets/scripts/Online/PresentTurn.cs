using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresentTurn : MonoBehaviour
{
    public int presentturnnum;
    public int presentturnplayer;
    TurnManagerOnline tmo;
    ModeData md;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        tmo = GameObject.Find("GameManager").GetComponent<TurnManagerOnline>();
        md = GameObject.Find("ModeData").GetComponent<ModeData>();
	  
    }

    // Update is called once per frame
    void Update()
    {
        
	  presentturnnum = tmo.turn;
	  presentturnplayer = tmo.turnPlayer;
	  text = GameObject.Find("presentPlayerNum").GetComponent<Text>();
	  text.text = "Turn Player = Player" + md.playerInfo[presentturnplayer];
    }
}
