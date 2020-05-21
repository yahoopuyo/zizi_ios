using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    Record record;
    private List<int> uniforms;
    private List<int> info;
    private List<int>[] handUniforms;
    //private List<int>[] originalUniforms;
    //private List<int>[] drawnUniform;
    //private List<int> opensource;
    public int playerNumber;
    public int computerLevel;
    public bool zizikakunum = false;
    public bool zizikakuplace = false;
    public bool successflag = false;

    /*
    ちょくちょく更新するので読んで！
    〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜
    棋譜...record.record -> 正方形,None=-1
    プライベート情報...info -> None=-1
    handuniform...handuniform[player_num]で、player_numの持ってる背番号
    〜〜〜〜〜〜〜〜〜〜〜〜〜4/10更新〜〜〜〜〜〜〜〜〜〜〜〜〜  
    record.GetDrawnUniform()[player_num]で、player_numの持ってるドローンの背番号
    record.GetOriginalUniform()[player_num]で、player_numの持ってるオリジナルの背番号
    record.opensource()で、opensourceにあるカード番号  
    (この３つは結構使うようならget()に入れてもらってもよい)
    draw()の帰り値は「背番号」に変更！！！！
    */

    private void get()
    {
        record = GameObject.Find("GameManager").GetComponent<Record>();
        info = record.info[playerNumber];
        handUniforms = record.GetHandUniform();
        uniforms = record.Uniform;

    }

    private int countN(List<int>[] rec,int uniform)
    {
        int cnt = 0;
        foreach (int un in rec[uniform]) if (un == -1) cnt++;
        return cnt;
    }

    private int publicZizikaku(List<int>[] rec)
    {
        get();
        int zizi = -1;
        foreach (int i in record.UniformExists)
        {
            if (countN(rec, i) == 0) zizi = i;
        }
        if (zizi != -1)
        {
            zizikakunum = true;
            zizikakuplace = true;
        }
        return zizi; //ziziの背番号を返す。なかったら-1。
    }

    private List<int> success(int drawnPlayer)
    {
        get();
        List<int> suc = new List<int>();
        foreach(int un in handUniforms[drawnPlayer])
        {
            if(info[un] != -1)
            {
                foreach(int myun in handUniforms[playerNumber])
                {
                    if(info[myun] % 13 == info[un] % 13)
                    {
                        suc.Add(un);
                        successflag = true;
                    }
                }
            }
        }

        return suc;
    }

    public int draw(int drawnPlayer)
    {
        get();
        int zizikamo = publicZizikaku(record.record);
        int CardUniform = 100;

        if (handUniforms[drawnPlayer].Count == 1) return handUniforms[drawnPlayer][0];

        //if (handUniforms[drawnPlayer].Contains(zizikamo)) handUniforms[drawnPlayer].Remove(zizikamo);

        List<int> suc = success(drawnPlayer);
        if (suc.Count != 0) CardUniform = suc[0];
        else
        {
            while (true)
            {
                int index = Random.Range(0, handUniforms[drawnPlayer].Count);
                CardUniform = handUniforms[drawnPlayer][index];
                if (zizikamo != CardUniform) break;
            }
        }

        return CardUniform;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
