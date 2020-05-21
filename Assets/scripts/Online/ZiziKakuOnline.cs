using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ZiziKakuOnline : MonoBehaviour
{
    //private List<GameObject> guessListCard;
    private List<int> guessListIndex;
    public List<int> debugList;
    public string comgListString;

    ScoreManagerOnline sm;
    TurnManagerOnline tm;
    ModeData md;

    [PunRPC]
    void SendGuessList(int turn, int playernum, string guessListString)
    {
        List<int> guessList = new List<int>();
        guessList.Add(turn + 100); //先頭にturn+100を挿入（100はziziと被らせないため)
        for (int i=0; i<(guessListString.Length / 2); i++)
        {
            int index = int.Parse(guessListString.Substring(2 * i, 2));
            guessList.Add(index);
        }
        debugList = new List<int>(guessList);
        sm.zzkkList[playernum] = guessList;
    }

    private bool zizikakued;



    public bool InGuessList(int cardIndex)
    {
        return guessListIndex.Contains(cardIndex);
    }

    public bool UpdateGuessList(int cardIndex) //return true when it deesn't need to ToggleFace
    {
        if (zizikakued) return true;//もうzizi確済みなら無視する
        if (guessListIndex.Contains(cardIndex))
        {
            guessListIndex.Remove(cardIndex);
            return false;
        }
        else
        {
            if (guessListIndex.Count > 5) return true;
            guessListIndex.Add(cardIndex);
            return false;
        }
    }

    public void RemoveFromGuessList(int cardIndex1, int cardIndex2)
    {
        guessListIndex.Remove(cardIndex1);
        guessListIndex.Remove(cardIndex2);
    }
    
    //じじかくボタン
    public void OnClicked()
    {
        if (guessListIndex.Count == 0) return;
        zizikakued = true;
        string gListString = "";
        foreach (int card in guessListIndex)
        {
            if (card < 10) gListString += "0" + card.ToString();
            else gListString += card.ToString(); 
        }
        PhotonView view = GetComponent<PhotonView>();
        view.RPC("SendGuessList", PhotonTargets.All, tm.turn, md.player, gListString);
        guessListIndex.Clear();
        GameObject.Find("ZizikakuButton").SetActive(false);
    }

    //comじじかく
    public void ComZizikaku(int card, int com)
    {
        if (!md.IsHost() || md.playerInfo[com] != "Com") return;
        if (tm.gameOver) return; //ゲーム終了後はじじかくできない
        comgListString = "";
        if (card < 10) comgListString += "0" + card.ToString();
        else comgListString += card.ToString();
        PhotonView view = GetComponent<PhotonView>();
        view.RPC("SendGuessList", PhotonTargets.All, tm.turn, com, comgListString);
    }

    // Start is called before the first frame update
    void Start()
    {
        guessListIndex = new List<int>();
        sm = GetComponent<ScoreManagerOnline>();
        tm = GetComponent<TurnManagerOnline>();
        md = GameObject.Find("ModeData").GetComponent<ModeData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
