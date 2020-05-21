using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerOnline : MonoBehaviour
{
    RecordOnline record;
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
    private bool CpuInitialized = false;

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
    load()を引いた後に毎回更新、  
    この前はエラーが多くて大変だったので、エラーになりそうだったら日本語で書いてくれると助かります
    */

    private void get()
    {
        record = GameObject.Find("GameManager").GetComponent<RecordOnline>();
        info = record.info[playerNumber];
        handUniforms = record.GetHandUniform();
        //uniforms = record.Uniform;
    }

    //更新情報
    private int previousMovedCard;
    private int previousDrawnPlayer;
    private int previousTurnPlayer;
    private int previousDeletedCard;

    public void load(int drawnPlayer, int carduniform, int turnPlayer, int deleted)
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
        previousDeletedCard = deleted;
        Publiczizikaku(record.record);
        Blankzizikaku();
    }

    private List<int> scoresfordraw; //背番号の数だけ５０点が入った数列を用意するまだ

    private int countN(List<int>[] rec, int uniform)
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


    private List<int>[] Blanklister(List<int>[] b0p, int blankindex)  //zizi候補配列を返す
    {
        int blankmod = blankmods[blankindex];

        List<int>[] c = new List<int>[4];
        for (int j = 0; j < 4; j++)
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

    private List<int>[] blistpublic = new List<int>[4];
    private List<List<int>[]> blistprivate = new List<List<int>[]>();
    private List<List<List<int>>> blanklist4 = new List<List<List<int>>>();
    private List<List<List<int>>> blanklist3 = new List<List<List<int>>>();
    private List<bool> PairChecked = new List<bool>(); //これはそのまま？

    private void InitBlankChaser(List<int>[] rec, int blankindex)
    {
        get();
        int blankmod = blankmods[blankindex];
        for (int j = 0; j < 4; j++) blistpublic[j] = handUniforms[j];  //ここもしかして間違ってる？

        blistprivate.Add(Blanklister(blistpublic, blankindex));
        //Debug.Log(blistprivate[0].Count);
        int pos4 = blistprivate[blankindex][0].Count * blistprivate[blankindex][1].Count * blistprivate[blankindex][2].Count * blistprivate[blankindex][3].Count;
        if (pos4 != 0)
        {
            foreach (int i in blistprivate[blankindex][0])
            {
                foreach (int j in blistprivate[blankindex][1])
                {
                    foreach (int k in blistprivate[blankindex][2])
                    {
                        foreach (int l in blistprivate[blankindex][3])
                        {
                            //Debug.Log(i);
                            //Debug.Log(j);
                            //Debug.Log(k);
                            //Debug.Log(l);
                            blanklist4[blankindex].Add(new List<int> { i, j, k, l });
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


        if (blanklist4[blankindex].Count == 0) //#blanklist4に含まれる１次元配列の個数カウントできてる？ //#turn0だけ特別に逆
        {
            zizinumber = blankmod;
            zizikakunum = true;
            if (blistprivate[blankindex][playerNumber].Count == 0) //このifは不要
            {
                //以下は仮で１，２，３としている
                List<int> playersTmp = new List<int> { 0, 1, 2, 3 };
                playersTmp.Remove(playerNumber);
                foreach (int j in blistprivate[blankindex][playersTmp[0]])
                {
                    foreach (int k in blistprivate[blankindex][playersTmp[1]])
                    {
                        foreach (int l in blistprivate[blankindex][playersTmp[2]])
                        {
                            blanklist3[blankindex].Add(new List<int> { j, k, l });
                        }
                    }
                }
            }
            //else Debug.Log("error"); //#debug用
        }
    }



    private void BlankChaser(List<int>[] rec, int blankindex)
    {
        get();
        int blankmod = blankmods[blankindex];
        List<int> knownBlanks = new List<int>();
        List<int> deadBlanks = new List<int>();
        foreach (int card in info) if (card % 13 == blankmod) knownBlanks.Add(info.IndexOf(card));  //opensourceにカードが出ていたら。
        foreach (int card in record.opensource()) if (card % 13 == blankmod) deadBlanks.Add(info.IndexOf(card));
        //Debug.Log(knownBlanks.Count);
        if (deadBlanks.Count == 4) return;  //全部出てたらいらない←めっちゃいい

        if (blanklist4.Count != 0) ////直感的にはこの部分特にRemoveで失敗してる気がする（blanklist4がうまく消せてない）
        {
            if (deadBlanks.Count == 2 && !PairChecked[blankindex]) //#blankmodがそろった（背番号match1とmatch2がそろった）
            {
                List<List<int>> tmplist2 = new List<List<int>>();
                foreach (List<int> kouho in blanklist4[blankindex])
                {
                    if (kouho.Contains(previousMovedCard) && kouho.Contains(previousDeletedCard)) tmplist2.Add(kouho);
                }
                blanklist4[blankindex] = new List<List<int>>(tmplist2);
                PairChecked[blankindex] = true;
            }
            else
            {
                if (previousDeletedCard != -1) //#そろった
                {
                    List<List<int>> tmplist = new List<List<int>>();
                    //Debug.Log(blanklist4[blankindex].Count);
                    foreach (List<int> kouho in blanklist4[blankindex])
                    {
                        if (kouho.Contains(previousMovedCard) && kouho.Contains(previousDeletedCard)) tmplist.Add(kouho);
                    }
                    foreach (List<int> kouho in tmplist) blanklist4[blankindex].Remove(kouho);
                    //Debug.Log(blanklist4[blankindex].Count);
                }
                else //#そろわず、移動したカードをmatch1として扱いmatch1が移動する直前のhanduniformsをmotomotとした→直前ではなく直後にした。→直前に直した
                {
                    List<int> motomoto = handUniforms[previousTurnPlayer];
                    motomoto.Remove(previousMovedCard);
                    List<List<int>> tmplist = new List<List<int>>();
                    //Debug.Log(blanklist4[blankindex].Count);
                    foreach (List<int> kouho in blanklist4[blankindex])
                    {
                        foreach (int uniform in motomoto)
                        {
                            if (kouho.Contains(previousMovedCard) && kouho.Contains(uniform)) tmplist.Add(kouho);
                        }
                    }
                    foreach (List<int> kouho in tmplist) blanklist4[blankindex].Remove(kouho);
                    //Debug.Log(blanklist4[blankindex].Count);
                }
            }
        }

        if (blanklist4.Count == 0)   //ziziかく
        {
            if (zizinumber == -1)  //ziziかくの瞬間
            {
                string debug = playerNumber + "じじ書く(２ターン目以降)";
                Debug.Log(debug);
                zizinumber = blankmod;
                zizikakunum = true;
                blistprivate[blankindex] = Blanklister(blistpublic, blankindex);

                int zeroPlayer = -1;
                for (int i = 0; i < 4; i++) if (blistprivate[blankindex][i].Count == 0) zeroPlayer = i;
                if (zeroPlayer != -1)
                {
                    //以下は仮で１，２，３としている
                    List<int> playersTmp = new List<int> { 0, 1, 2, 3 };
                    playersTmp.Remove(zeroPlayer);
                    foreach (int j in blistprivate[blankindex][playersTmp[0]])
                    {
                        foreach (int k in blistprivate[blankindex][playersTmp[1]])
                        {
                            foreach (int l in blistprivate[blankindex][playersTmp[2]])
                            {
                                blanklist3[blankindex].Add(new List<int> { j, k, l });
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
                        if (i != playerNumber)
                        {
                            List<int> tmp = new List<int> { 0, 1, 2, 3 };
                            tmp.Remove(i);
                            playersTmp.Add(tmp);
                        }
                    }
                    foreach (List<int> vs in playersTmp)
                    {
                        foreach (int j in blistprivate[blankindex][vs[0]])
                        {
                            foreach (int k in blistprivate[blankindex][vs[1]])
                            {
                                foreach (int l in blistprivate[blankindex][vs[2]])
                                {
                                    blanklist3[blankindex].Add(new List<int> { j, k, l });
                                }
                            }
                        }
                    }
                }

                List<List<int>> tmplist = new List<List<int>>();
                foreach (List<int> kouho in blanklist3[blankindex])
                {
                    if (rec[kouho[0]][kouho[1]] != -1) if (!knownBlanks.Contains(kouho[0]) || !knownBlanks.Contains(kouho[1])) { tmplist.Add(kouho); continue; }
                    if (rec[kouho[0]][kouho[2]] != -1) if (!knownBlanks.Contains(kouho[0]) || !knownBlanks.Contains(kouho[2])) { tmplist.Add(kouho); continue; }
                    if (rec[kouho[1]][kouho[2]] != -1) if (!knownBlanks.Contains(kouho[1]) || !knownBlanks.Contains(kouho[2])) { tmplist.Add(kouho); continue; }
                }
                foreach (List<int> kouho in tmplist) blanklist3[blankindex].Remove(kouho);
            }
            else　//#以前にziziかくしてた場合
            {
                if (deadBlanks.Count == 2 && !PairChecked[blankindex]) //#blankmodがそろった（背番号match1とmatch2がそろった）
                {
                    List<List<int>> tmplist2 = new List<List<int>>();
                    foreach (List<int> kouho in blanklist3[blankindex])
                    {
                        if (kouho.Contains(previousMovedCard) && kouho.Contains(previousDeletedCard)) tmplist2.Add(kouho);
                    }
                    blanklist3[blankindex] = new List<List<int>>(tmplist2);
                    PairChecked[blankindex] = true;
                }
                else
                {
                    if (previousDeletedCard != -1) //#そろった
                    {
                        List<List<int>> tmplist3 = new List<List<int>>();
                        //Debug.Log(blanklist3[blankindex].Count);
                        foreach (List<int> kouho in blanklist3[blankindex])
                        {
                            if (kouho.Contains(previousMovedCard) && kouho.Contains(previousDeletedCard)) tmplist3.Add(kouho);
                        }
                        foreach (List<int> kouho in tmplist3) blanklist3[blankindex].Remove(kouho);
                        //Debug.Log(blanklist3[blankindex].Count);
                    }
                    else //#そろわず、移動したカードをmatch1として扱いmatch1が移動する直前のhanduniformsをmotomotとした→直前ではなく直後にした。
                    {
                        List<int> motomoto = handUniforms[previousTurnPlayer];
                        motomoto.Remove(previousMovedCard);
                        List<List<int>> tmplist4 = new List<List<int>>();
                        //Debug.Log(blanklist3[blankindex].Count);
                        foreach (List<int> kouho in blanklist3[blankindex])
                        {
                            foreach (int uniform in motomoto)
                            {
                                if (kouho.Contains(previousMovedCard) && kouho.Contains(uniform)) tmplist4.Add(kouho);
                            }
                        }
                        foreach (List<int> kouho in tmplist4) blanklist3[blankindex].Remove(kouho);
                        //Debug.Log(blanklist3.Count);
                    }
                }
            }
        }
        //Debug.Log(blanklist4[blankindex].Count);
        //return; //どうする？
    }

    //ここまでブランクziziかく

    private List<int> success(int drawnPlayer)
    {
        get();
        List<int> suc = new List<int>();
        foreach (int un in handUniforms[drawnPlayer])
        {
            if (info[un] != -1)
            {
                foreach (int myun in handUniforms[playerNumber])
                {
                    if (info[myun] % 13 == info[un] % 13)
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
        if (blankmods.Count == 0) return;  //blankindex付いてたのはミス？  
        int debugcnt = 0;
        for (int j = 0; j < blankmods.Count; j++)
        {
            BlankChaser(record.record, j);
            if (zizinumber == blankmods[j] && ziziuniform == -1)
            {
                if (blanklist3[j].Count == 1)
                {
                    foreach (int uni in blanklist3[j][0])
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
            if (debugcnt > 1) Debug.Log("zizikakued though more than 2 exists");  //本当は３枚ある場合も引かないようにしたい
        }
    }











    public int draw(int drawnPlayer)
    {
        get();
        //Blankzizikaku();
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
        
    }// Update is called once per frame
    void Update()
    {
        if(!CpuInitialized)
        {
            record = GameObject.Find("GameManager").GetComponent<RecordOnline>();
            if (record.Initialized)
            {
                get();
                List<int> gravenum = new List<int>();
                foreach (int card in record.opensource()) gravenum.Add(card % 13);
                for (int num = 0; num < 13; num++)
                {
                    if (!gravenum.Contains(num)) blankmods.Add(num);
                }
                for (int i = 0; i < blankmods.Count; i++)
                {
                    PairChecked.Add(false);
                    blanklist4.Add(new List<List<int>>());
                    blanklist3.Add(new List<List<int>>());
                    InitBlankChaser(record.record, i);
                }
                CpuInitialized = true;
            }
        }
    }
}
