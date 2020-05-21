//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerAki : MonoBehaviour
{
    Record record;
    private List<int> info;
    private List<int>[] handUniforms;
    public int playerNumber;
    public int computerLevel;
    public List<int> blankmods;               //ブランクの数字で１３なら０としておく
    public bool zizikakunum = false;
    public bool zizikakuplace = false;
    public bool successflag = false;
    private int zizinumber = -1;
    private int ziziuniform = -1;

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
    〜〜〜〜〜〜〜〜〜〜〜〜4/16更新〜〜〜〜〜〜〜〜〜〜〜〜
    private変数として、previousTurnPlayer,previousDrawnPlayer,previousMovedCardを追加
    中身はそれぞれ一個前のターンで誰が引いたか、誰がひかれたか、何を引いたか
    この前はエラーが多くて大変だったので、エラーになりそうだったら日本語で書いてくれると助かります
    */

    private void get()
    {
        record = GameObject.Find("GameManager").GetComponent<Record>();
        info = record.info[playerNumber];
        handUniforms = record.GetHandUniform();
        //uniforms = record.Uniform;
    }

    //更新情報
    private int  previousMovedCard;
    private int previousDrawnPlayer;
    private int previousTurnPlayer;


    public void load(int drawnPlayer,int carduniform, int turnPlayer)
    {
        /*
        更新されるタイミングは、誰かがカードを引いて、record諸々更新された後なので、BlankChaserで使われる際は
        一個前のターンで誰が引いたか、誰がひかれたか、何を引いたか
        が格納されるようになっています。      
        */
        get();
        previousMovedCard = carduniform;
        previousDrawnPlayer = drawnPlayer;
        previousTurnPlayer = turnPlayer;
        Publiczizikaku(record.record);
        Blankzizikaku();
    }

    private List<int> scoresfordraw; //背番号の数だけ５０点が入った数列を用意するまだ

    private int countN(List<int>[] rec,int uniform)
    {
        int cnt = 0;
        foreach (int un in rec[uniform]) if (un == -1) cnt++;
        return cnt;
    }

    //zizikaku関数についてはvoid型にしたほうがメモリの節約になっていいと思う

    private void Publiczizikaku(List<int>[] rec)
    {
        get();
        foreach (int i in record.UniformExists)
        {
            if (countN(rec, i) == 0) ziziuniform = i;
        }
        if (ziziuniform != -1)
        {
            zizikakuplace = true;
            if (info[ziziuniform] != -1)
            {
                zizinumber = info[ziziuniform] % 13;
                zizikakunum = true;
            }
        }
        //return zizi; //ziziの背番号を返す。なかったら-1。
    }

    
    private List<int>[] Blanklister(List<int>[] b0p,int blankindex)  //zizi候補配列を返す
    {
        int blankmod = blankmods[blankindex];

        List<int>[] c = new List<int>[4];
        for (int j=0; j<4; j++)
        {
            int d = -1;
            c[j] = new List<int>();
            foreach (int i in b0p[j])   //#a[j]が空ならc[j]={}としたい
            {
                if (info[i] % 13 == blankmod) d = i;
                else
                {
                    if (info[i] == -1) c[j].Add(i);
                }
            }
            if (d != -1) c[j] = new List<int> { d };
        }
        return c;
    }


    //blankzizi用

    private List<int>[] blistpublic;
    private List<int>[] blistprivate;
    private List<List<int>> blanklist4 = new List<List<int>>();
    private List<List<int>> blanklist3 = new List<List<int>>();
    private List<bool> PairChecked = new List<bool>();

    private void InitBlankChaser(List<int>[] rec,int blankindex)
    {
        get();
        int blankmod = blankmods[blankindex];
        blistpublic = new List<int>[4]; //#b0pのこと
        for (int j = 0; j < 4; j++) blistpublic[j] = handUniforms[j];

        blistprivate = Blanklister(blistpublic,blankindex);
        //Debug.Log(blistprivate[0].Count);
        int pos4 = blistprivate[0].Count * blistprivate[1].Count * blistprivate[2].Count * blistprivate[3].Count;
        if (pos4 != 0)  //ziziかくしていてposが0ならblanklist4={ {} }としたい
        {
            foreach (int i in blistprivate[0])
            {
                foreach (int j in blistprivate[1])
                {
                    foreach (int k in blistprivate[2])
                    {
                        foreach (int l in blistprivate[3])
                        {
                            //Debug.Log(i);
                            //Debug.Log(j);
                            //Debug.Log(k);
                            //Debug.Log(l);
                            blanklist4.Add(new List<int> { i, j, k, l });
                        }
                    }
                }
            }
        }
        //foreach(List<int> a in blanklist4)
        //{
        //    foreach (int b in a) Debug.Log(b);
        //    Debug.Log("\n");
        //}


        if (blanklist4.Count == 0) //#blanklist4に含まれる１次元配列の個数カウントできてる？ //#turn0だけ特別に逆
        {
            zizinumber = blankmod;
            zizikakunum = true;
            if (blistprivate[playerNumber].Count == 0)
            {
                //以下は仮で１，２，３としている
                List<int> playersTmp = new List<int> { 0, 1, 2, 3 };
                playersTmp.Remove(playerNumber);
                foreach (int j in blistprivate[playersTmp[0]])
                {
                    foreach (int k in blistprivate[playersTmp[1]])
                    {
                        foreach (int l in blistprivate[playersTmp[2]])
                        {
                            blanklist3.Add(new List<int> { j, k, l });
                        }
                    }
                }
            }
            else  Debug.Log("error"); //#debug用
        }
    }

    private void BlankChaser(List<int>[] rec,int blankindex)
    {
        //int match1 = draw();  //drawはどこのクラスに入ってるの？
        //int match2 = -1;
        //if (   )  //そろった
        //{
        //    match2 = ;  //相方の背番号
        //}

        get();
        int blankmod = blankmods[blankindex];
        List<int> knownBlanks = new List<int>();
        //foreach(int card in info) if (card % 13 == blankmod) knownBlanks.Add(info.IndexOf(card));  //opensourceにカードが出ていたら。
        foreach (int card in record.opensource()) if (card % 13 == blankmod) knownBlanks.Add(info.IndexOf(card));
        Debug.Log(knownBlanks.Count);
        if (knownBlanks.Count == 4) return;  //全部出てたらいらない

        if (blanklist4.Count != 0)
        {
            if (knownBlanks.Count >= 2 && !PairChecked[blankindex]) //#blankmodがそろった（背番号match1とmatch2がそろった）
            {
                List<List<int>> tmplist2 = new List<List<int>>();
                foreach (List<int> kouho in blanklist4)
                {
                    if (knownBlanks.Count == 3) if(!kouho.Contains(knownBlanks[0]) || !kouho.Contains(knownBlanks[1]) || !kouho.Contains(knownBlanks[2])) { tmplist2.Add(kouho); continue; }
                    if (!kouho.Contains(knownBlanks[0]) || !kouho.Contains(knownBlanks[1])) tmplist2.Add(kouho);
                }
                foreach (List<int> kouho in tmplist2) blanklist4.Remove(kouho);
                PairChecked[blankindex] = true;
                //for (int j = 0; j < blanklist4.Count; j++) 
                //{
                //    int a = 0;
                //    int b = 0;
                //    for (int i = 0; i < 4; i++)
                //    {
                //        if (blanklist4[j][i] == match1)
                //        {
                //            a = 1; 
                //        }
                //        if (blanklist4[j][i] == match2)
                //        {
                //            b = 1;
                //        }
                //    }
                //    if (a * b != 1)　//#match1とmatch2が共存して無ければ
                //    {
                //        blanklist4[j] = null; //本当は消したい
                //    }
                //}
            }
            List<List<int>> tmplist = new List<List<int>>();
            Debug.Log(blanklist4.Count);
            foreach (List<int> kouho in blanklist4)
            {
                if (rec[kouho[0]][kouho[1]] != -1) if (!knownBlanks.Contains(kouho[0]) && !knownBlanks.Contains(kouho[1])) { tmplist.Add(kouho); Debug.Log(-1); continue; }
                if (rec[kouho[0]][kouho[2]] != -1) if (!knownBlanks.Contains(kouho[0]) && !knownBlanks.Contains(kouho[2])) { tmplist.Add(kouho); Debug.Log(-2); continue; }
                if (rec[kouho[0]][kouho[3]] != -1) if (!knownBlanks.Contains(kouho[0]) && !knownBlanks.Contains(kouho[3])) { tmplist.Add(kouho); Debug.Log(-3); continue; }
                if (rec[kouho[1]][kouho[2]] != -1) if (!knownBlanks.Contains(kouho[1]) && !knownBlanks.Contains(kouho[2])) { tmplist.Add(kouho); Debug.Log(-4); continue; }
                if (rec[kouho[1]][kouho[3]] != -1) if (!knownBlanks.Contains(kouho[1]) && !knownBlanks.Contains(kouho[3])) { tmplist.Add(kouho); Debug.Log(-5); continue; }
                if (rec[kouho[2]][kouho[3]] != -1) if (!knownBlanks.Contains(kouho[2]) && !knownBlanks.Contains(kouho[3])) { tmplist.Add(kouho); Debug.Log(-6); continue; }
            }
            foreach(List<int> kouho in tmplist) blanklist4.Remove(kouho);
            Debug.Log(blanklist4.Count);
        }

        //    else
        //    {
        //        if (    ) //#そろった
        //        {
        //            for (int j = 0; j < blanklist4.Count; j++)
        //            {
        //                int a = 0;
        //                int b = 0;
        //                for (int i = 0; i < 4; i++)
        //                {
        //                    if (blanklist4[j][i] == match1)
        //                    {
        //                        a = 1;
        //                    }
        //                    if (blanklist4[j][i] == match2)
        //                    {
        //                        b = 1;
        //                    }
        //                }
        //                if (a * b == 1) //#match1とmatch2のいずれか一方があれば
        //                {
        //                    blanklist4[j] = null; //本当は消したい
        //                }
        //            }
        //        }
        //        else　//#そろわず、移動したカードをmatch1として扱いmatch1が移動する直前のhanduniformsをmotomotとした
        //        {
        //            List<int> motomoto = handUniforms[引くplayerNumber];　//おはぎ
        //            for (int j = 0; j < blanklist4.Count; j++)
        //            {
        //                int a = 0;
        //                int b = 0;
        //                for (int i = 0; i < 4; i++)
        //                {
        //                    if (blanklist4[j][i] == match1)
        //                    {
        //                        a = 1;
        //                    }
        //                    foreach (int k in motomoto)
        //                    {
        //                        if (blanklist4[j][i] == k)
        //                        {
        //                            b = 1;
        //                        }                     
        //                    }
        //                }
        //                if (a * b == 1) //#match1とkがともにあれば
        //                {
        //                    blanklist4[j] = null; //本当は消したい
        //                }
        //            }
        //        }
        //    }
        //}
        if(blanklist4.Count==0)   //ziziかく
        {
            if (zizinumber == -1)  //ziziかくの瞬間
            {
                string debug = playerNumber + "じじ書く(２ターン目以降)";
                Debug.Log(debug);
                zizinumber = blankmod;
                zizikakunum = true;
                blistprivate = Blanklister(blistpublic, blankindex);

                int zeroPlayer = -1;
                for (int i = 0; i < 4; i++) if (blistprivate[i].Count == 0) zeroPlayer = i;
                if (zeroPlayer != -1)
                {
                    //以下は仮で１，２，３としている
                    List<int> playersTmp = new List<int> { 0, 1, 2, 3 };
                    playersTmp.Remove(zeroPlayer);
                    foreach (int j in blistprivate[playersTmp[0]])
                    {
                        foreach (int k in blistprivate[playersTmp[1]])
                        {
                            foreach (int l in blistprivate[playersTmp[2]])
                            {
                                blanklist3.Add(new List<int> { j, k, l });
                            }
                        }
                    }
                }
                else
                {
                    //４つから３つをえらばないといけない仮で１，２，３としている、、かなり面倒
                    List<List<int>> playersTmp = new List<List<int>>();
                    for (int i = 0; i < 4; i++)
                    {
                        if(i != playerNumber)
                        {
                            List<int> tmp = new List<int> { 0, 1, 2, 3 };
                            tmp.Remove(i);
                            playersTmp.Add(tmp);
                        }
                    }
                    foreach (List<int> vs in playersTmp)
                    {
                        foreach (int j in blistprivate[vs[0]])
                        {
                            foreach (int k in blistprivate[vs[1]])
                            {
                                foreach (int l in blistprivate[vs[2]])
                                {
                                    blanklist3.Add(new List<int> { j, k, l });
                                }
                            }
                        }
                    }
                }

                List<List<int>> tmplist = new List<List<int>>();
                foreach (List<int> kouho in blanklist3)
                {
                    if (rec[kouho[0]][kouho[1]] != -1) if (!knownBlanks.Contains(kouho[0]) || !knownBlanks.Contains(kouho[1])) { tmplist.Add(kouho); continue; }
                    if (rec[kouho[0]][kouho[2]] != -1) if (!knownBlanks.Contains(kouho[0]) || !knownBlanks.Contains(kouho[2])) { tmplist.Add(kouho); continue; }
                    if (rec[kouho[1]][kouho[2]] != -1) if (!knownBlanks.Contains(kouho[1]) || !knownBlanks.Contains(kouho[2])) { tmplist.Add(kouho); continue; }
                }
                foreach (List<int> kouho in tmplist) blanklist3.Remove(kouho);
                //for (int j = 0; j < blanklist3.Count; j++) //#共存情報から消去
                //{
                //    for (int i=0; i < 3; i++)
                //    {
                //        if (rec[j][i] != -1)
                //        {
                //            blanklist3[j] = null; //本当は消したい
                //        }                      
                //    }
                //}
            }
            else　//#以前にziziかくしてた場合
            {
                List<List<int>> tmplist = new List<List<int>>();
                foreach (List<int> kouho in blanklist3)
                {
                    if (rec[kouho[0]][kouho[1]] != -1 || rec[kouho[0]][kouho[2]] != -1 || rec[kouho[1]][kouho[2]] != -1) tmplist.Add(kouho);
                }
                foreach (List<int> kouho in tmplist) blanklist3.Remove(kouho);
                //if (    ) //#blankmodがそろった（背番号match1とmatch2がそろった）
                //{
                //for (int j = 0; j < blanklist3.Count; j++)
                //{
                //        int a = 0;
                //        int b = 0;
                //        for (int i = 0; i < 3; i++)
                //        {
                //            if (blanklist3[j][i] == match1)
                //            {
                //                a = 1;
                //            }
                //            if (blanklist3[j][i] == match2)
                //            {
                //                b = 1;
                //            }
                //        }
                //        if (a * b != 1) //#match1とmatch2が共存して無ければ
                //        {
                //            blanklist3[j] = null; //本当は消したい
                //        }
                //    }
                //}
                //else
                //{
                //    if (     ) //#そろった
                //    {
                //        for (int j = 0; j < blanklist3.Count; j++)
                //        {
                //            int a = 0;
                //            int b = 0;
                //            for (int i = 0; i < 3; i++)
                //            {
                //                if (blanklist3[j][i] == match1)
                //                {
                //                    a = 1;
                //                }
                //                if (blanklist3[j][i] == match2)
                //                {
                //                    b = 1;
                //                }
                //            }
                //            if (a * b == 1) //#match1とmatch2のどちらか一方があれば
                //            {
                //                blanklist3[j] = null; //本当は消したい
                //            }
                //        }
                //    }
                //    else　//#そろわず、移動したカードをmatch1として扱いmatch1が移動する直前のhanduniformsをmotomotoとした
                //    {
                //        for (int j = 0; j < blanklist3.Count; j++)
                //        {
                //            int a = 0;
                //            int b = 0;
                //            for (int i = 0; i < 3; i++)
                //            {
                //                if (blanklist3[j][i] == match1)
                //                {
                //                    a = 1;
                //                }
                //                foreach (int k in motomoto)
                //                {
                //                    if (blanklist3[j][i] == k)
                //                    {
                //                        b = 1;
                //                    }
                //                }
                //            }
                //            if (a * b == 1) //#match1とkがともにあれば
                //            {
                //                blanklist3[j] = null; //本当は消したい
                //            }
                //        }
                //    }
                //}
            }
        }
        //return; //どうする？
    }

    //ここまでブランクziziかく

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

    private void Blankzizikaku()
    {
        if (blankmods.Count == 0) return;
        BlankChaser(record.record, 0);
        int debugcnt = 0;
        if (zizinumber == blankmods[0] && ziziuniform==-1)
        {
            if(blanklist3.Count == 1)
            {
                foreach(int uni in blanklist3[0])
                {
                    if (record.UniformExists.Contains(uni))
                    {
                        ziziuniform = uni;
                        zizikakuplace = true;
                        debugcnt++;
                    }
                }
            }
        }
        if (debugcnt > 1) Debug.Log("zizikakued though more than 2 exists");
    }











    public int draw(int drawnPlayer)
    {
        get();
        Blankzizikaku();
        Publiczizikaku(record.record);
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
                if (ziziuniform != CardUniform) break;
            }
        }

        return CardUniform;
    }



    // Start is called before the first frame update
    void Start()
    {
        get();
        List<int> gravenum = new List<int>();
        foreach (int card in record.opensource()) gravenum.Add(card % 13);
        for (int num=0; num < 13; num++)
        {
            if (!gravenum.Contains(num)) blankmods.Add(num);
        }
        for(int i = 0; i < blankmods.Count; i++)
        {
            PairChecked.Add(false);
            InitBlankChaser(record.record,i);
        }

    }// Update is called once per frame
    void Update()
    {
        
    }
}
