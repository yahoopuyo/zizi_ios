using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ZiziKakuOnline : MonoBehaviour
{
    //private List<GameObject> guessListCard;
    private List<int> guessListIndex;
    public List<int> debugList;
    public string comgListString;

    ScoreManagerOnline sm;
    TurnManagerOnline tm;
    ModeData md;

    public bool selectMode = false;  //ziziを選択するモード
    private bool zizikakued;

    [SerializeField] public GameObject zizikakuBtn;

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

    private void FlipBtnColor(bool red)
    {
        if(red)
        {
            zizikakuBtn.GetComponent<Image>().color = Color.red;
        }
        else
        {
            zizikakuBtn.GetComponent<Image>().color = Color.white;
        }
    }

    //じじかくボタン
    public void OnClicked()
    {
        if (guessListIndex.Count == 0)
        {
            selectMode = !selectMode;
            FlipBtnColor(selectMode);
            return;
        }
        zizikakued = true;
        selectMode = false;
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
