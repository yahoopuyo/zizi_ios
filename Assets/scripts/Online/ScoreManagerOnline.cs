using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManagerOnline : MonoBehaviour
{
    ModeData md;
    TurnManagerOnline tm;
    List<int> score;
    List<int> zzkkscore;
    List<string> result = new List<string>();
    public List<int>[] zzkkList = new List<int>[4];
    private List<int> zzkkrank = new List<int>() { 0, 0, 0, 0 };
    [SerializeField] private GameObject gameoverP;
    [SerializeField] private GameObject canvas4;

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<TurnManagerOnline>();
        md = GameObject.Find("ModeData").GetComponent<ModeData>();
        
    }

    public void WriteResult(List<int> wins, int zizi)
    {
        List<int> scoreToOrder = new List<int>() { 120, 70, 60, 0};
        List<int> zzkkscoreToOrder = new List<int>() { 120, 90, 60, 30};
        canvas4.SetActive(false);
        gameoverP.SetActive(true);
        score = md.score;
        zzkkscore = new List<int> { 0, 0, 0, 0 };
        string[] playerInfo = md.playerInfo;
        string[] prePlayerInfo = md.prePlayerInfo;
        string order;
        string points;
        
        for (int i = 0; i < 4; i++)
        {
            //じじかくぼたんおさなかった場合の処置やけどそもそもzzkkList[3]とかがなかってもいいのかな？
            //無理そうならSendGuessList()でInsertやめてAddにしてもう一ループしてzzkkList再編する
            if (zzkkList[i] == null) zzkkList[i] = new List<int> { 1000 };
            else
            {
                if (!zzkkList[i].Contains(zizi)) zzkkList[i][0] = 1000;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (zzkkList[i][0] > zzkkList[j][0]) //ターン番号の小さいものがあれば
                {
                    zzkkrank[i] += 1;
                }
            }
            if (zzkkList[i][0] == 1000) zzkkscore[i] = 0; //じじかくボタン押さなかった場合もこっちに入る
            else zzkkscore[i] += zzkkscoreToOrder[zzkkrank[i]] / (zzkkList[i].Count - 1);
        }
        
        for (int i=0; i < 4; i++)
        {
            score[wins[i]] += scoreToOrder[i];
            result.Add("No." + (i+1) + ": " + playerInfo[wins[i]]);
        }
        
        order = result[0] + "\n" + result[1] + "\n" + result[2] + "\n" + result[3];
        points = "\n\n" + playerInfo[0] + ": " + score[0] + " + " + zzkkscore[0] + " pt,     " + playerInfo[1] + ": " + score[1] + " + " + zzkkscore[1] + " pt"+ "\n" + playerInfo[2] +": "+ score[2] + " + " + zzkkscore[2] + " pt,     " + playerInfo[3] +": " + score[3] + " + " + zzkkscore[3] + " pt";
        Text text = GameObject.Find("Results").GetComponent<Text>();
        text.text = order + points;
        for (int i = 0; i < 4; i++)
        {
            score[i] += zzkkscore[i];
        }

        if (!md.IsHost()) GameObject.Find("RestartButton").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
