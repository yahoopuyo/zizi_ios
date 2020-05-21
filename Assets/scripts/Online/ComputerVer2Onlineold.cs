using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerVer2Onlineold : MonoBehaviour
{
    RecordOnline record;
    private List<int> info;
    private List<int>[] handUniforms;
    public int playerNumber;
    public int computerLevel;
    public List<int> blankmods;               //ブランクの数字 - 1
    public bool zizikakunum = false;
    public bool zizikakuplace = false;
    public bool successflag = false;
    private int zizinumber = -1;
    private int ziziuniform = -1;
    private bool CpuInitialized = false;

    /*
    〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜〜
    棋譜...record.record -> 正方形,None=-1
    プライベート情報...info -> None=-1
    handuniform...handuniforms[player_num]で、player_numの持ってる背番号
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
        ownrecord();
        Publiczizikaku(record.record);
        Blankzizikaku();
    }

    private List<int> scoresfordraw; //背番号の数だけ５０点が入った数列を用意するまだ

    private void ownrecord() //publiczizikakuをそのまま使ってprivatezizizkakuを実装した,直接record.recordを更新することに変えた
    {
        if (previousTurnPlayer == playerNumber) //comの番のとき
        {
            if (previousDeletedCard == -1) //引いて揃わなかった、どこで初期化ー１にした？？
            {
                for (int j = 0; j < info.Count; j++)
                {
                    if (info[j] != -1 && info[previousMovedCard] % 13 != info[j]) //揃わない場合だけ、infoUsingBlankを使いたいけど複雑化するから保留
                    {
                        record.record[previousMovedCard][j] = 99;
                        record.record[j][previousMovedCard] = 99;
                    }
                }
            }
        }
    }

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
        for (int j = 0; j < 4; j++) blistpublic[j] = handUniforms[j];

        blistprivate.Add(Blanklister(blistpublic, blankindex));
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
                            blanklist4[blankindex].Add(new List<int> { i, j, k, l });
                            //if (blanklist4[blankindex].Count<10)
                            //{
                            //Debug.Log(i);
                            //Debug.Log(j);
                            //Debug.Log(k);
                            //Debug.Log(l);
                            //}
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

    private List<int> nonsuccessturn = new List<int>();

    private void BlankChaser(List<int>[] rec, int blankindex)
    {
        get();
        int blankmod = blankmods[blankindex];
        //Debug.Log(blankmod);
        List<int> knownBlanks = new List<int>();
        List<int> deadBlanks = new List<int>();
        foreach (int card in info) if (card % 13 == blankmod) knownBlanks.Add(info.IndexOf(card));  //opensourceにカードが出ていたら。
        foreach (int card in record.opensource()) if (card % 13 == blankmod) deadBlanks.Add(info.IndexOf(card));

        if (deadBlanks.Count == 4) return;  //全部出てたらいらない←めっちゃいい

        if (blanklist4[blankindex].Count != 0)
        {
            if (deadBlanks.Count == 2 && !PairChecked[blankindex]) //#blankmodがそろった
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
                        if (kouho.Contains(previousMovedCard) || kouho.Contains(previousDeletedCard)) tmplist.Add(kouho);
                    }
                    foreach (List<int> kouho in tmplist) blanklist4[blankindex].Remove(kouho);
                    //Debug.Log(blanklist4[blankindex].Count);
                }
                else //#そろわず、移動したカードをpreviousMovedCardとして扱い移動する直前のhanduniformsをmotomotoとした
                {
                    List<int> motomoto = handUniforms[previousTurnPlayer];
                    motomoto.Remove(previousMovedCard);
                    //Debug.Log("そろわず");
                    if (!nonsuccessturn.Contains(rec[previousMovedCard][motomoto[0]]))
                    {
                        nonsuccessturn.Add(rec[previousMovedCard][motomoto[0]]);
                    }//turn番号の取得方法が分からなかったからとりあえず棋譜から読むことにした

                    if (previousTurnPlayer == playerNumber) /////comの番のとき
                    {
                        if (info[previousMovedCard] % 13 == blankmod) //引いたカードがブランクだったら
                        {
                            List<List<int>> tmplist = new List<List<int>>();
                            foreach (List<int> kouho in blanklist4[blankindex])
                            {
                                if (!kouho.Contains(previousMovedCard)) tmplist.Add(kouho);
                            }
                            foreach (List<int> kouho in tmplist) blanklist4[blankindex].Remove(kouho);
                        }
                        else
                        {
                            List<List<int>> tmplist = new List<List<int>>();
                            foreach (List<int> kouho in blanklist4[blankindex])
                            {
                                if (kouho.Contains(previousMovedCard)) tmplist.Add(kouho);
                            }
                            foreach (List<int> kouho in tmplist) blanklist4[blankindex].Remove(kouho);
                        }
                    }
                    else
                    {
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
        }

        if (blanklist4[blankindex].Count == 0)   //ziziかく
        {
            if (zizinumber == -1 || blanklist3[blankindex].Count == 0)  //ziziかくの瞬間、publiczizizkakuしててもこっちに入れるようにした
            {
                string debug = playerNumber + "zizi確した！(数字)";
                Debug.Log(debug);
                zizinumber = blankmod;
                zizikakunum = true;

                blistprivate[blankindex] = Blanklister(blistpublic, blankindex);
                //List<List<int>[]> blistprivate = new List<List<int>[]>();
                //blistprivate.Add(Blanklister(blistpublic, blankindex));

                int zeroPlayer = -1;
                for (int i = 0; i < 4; i++) if (blistprivate[blankindex][i].Count == 0) zeroPlayer = i; //ここちょっと怪しかったけどたぶんあってる
                if (zeroPlayer != -1)
                {
                    Debug.Log("誰かのオリジナルすべて見てブランクなし");
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
                    //Debug.Log(blanklist3[blankindex].Count + 1000);
                }
                else
                {
                    //４つから３つをえらばないといけない
                    Debug.Log("誰かのオリジナルをすべて見てはいない");
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

                Debug.Log(nonsuccessturn.Count + 0.1);
                //foreach (int b in nonsuccessturn) Debug.Log(b);
                //Debug.Log(nonsuccessturn[0]);

                List<List<int>> tmplist = new List<List<int>>();　//以下うまく消せてない模様....
                foreach (List<int> kouho in blanklist3[blankindex])　//下記のコメントアウトのようにではなく、共存したら（かつ揃わなかったら）消去するようにした
                {
                    if (nonsuccessturn.Contains(rec[kouho[0]][kouho[1]])) { tmplist.Add(kouho); continue; }
                    if (nonsuccessturn.Contains(rec[kouho[0]][kouho[2]])) { tmplist.Add(kouho); continue; }
                    if (nonsuccessturn.Contains(rec[kouho[1]][kouho[2]])) { tmplist.Add(kouho); continue; }
                }
                foreach (List<int> kouho in tmplist) blanklist3[blankindex].Remove(kouho);
                //List<List<int>> tmplist = new List<List<int>>();
                //foreach (List<int> kouho in blanklist3[blankindex])　//////たぶん間違ってる
                //{
                //    if (rec[kouho[0]][kouho[1]] != -1) if (!knownBlanks.Contains(kouho[0]) || !knownBlanks.Contains(kouho[1])) { tmplist.Add(kouho); continue; }
                //    if (rec[kouho[0]][kouho[2]] != -1) if (!knownBlanks.Contains(kouho[0]) || !knownBlanks.Contains(kouho[2])) { tmplist.Add(kouho); continue; }
                //    if (rec[kouho[1]][kouho[2]] != -1) if (!knownBlanks.Contains(kouho[1]) || !knownBlanks.Contains(kouho[2])) { tmplist.Add(kouho); continue; }
                //}
                //foreach (List<int> kouho in tmplist) blanklist3[blankindex].Remove(kouho);


                //以下は雑に作った、もしかすると不要なものがあるかもしれない
                if (knownBlanks.Count == 1)
                {
                    Debug.Log("knownBlanks.Countが１");
                    List<List<int>> tmplist2 = new List<List<int>>();
                    foreach (List<int> kouho in blanklist3[blankindex])
                    {
                        if (!kouho.Contains(knownBlanks[0])) { tmplist2.Add(kouho); continue; }
                    }
                    foreach (List<int> kouho in tmplist2) blanklist3[blankindex].Remove(kouho);
                }
                if (knownBlanks.Count == 2)
                {
                    Debug.Log("knownBlanks.Countが２");
                    List<List<int>> tmplist2 = new List<List<int>>();
                    foreach (List<int> kouho in blanklist3[blankindex])
                    {
                        if (!kouho.Contains(knownBlanks[0]) || !kouho.Contains(knownBlanks[1])) { tmplist2.Add(kouho); continue; }
                    }
                    foreach (List<int> kouho in tmplist2) blanklist3[blankindex].Remove(kouho);
                }
                if (knownBlanks.Count == 3) //knownblanksで置き換えてしまった
                {
                    Debug.Log("knownBlanks.Countが３");
                    blanklist3[blankindex] = new List<List<int>> { knownBlanks };
                }
            }
            else
            {
                //Debug.Log("すでにzizi確");
                if (deadBlanks.Count == 2 && !PairChecked[blankindex]) //#blankmodがそろった
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
                            if (kouho.Contains(previousMovedCard) || kouho.Contains(previousDeletedCard)) tmplist3.Add(kouho);
                        }
                        foreach (List<int> kouho in tmplist3) blanklist3[blankindex].Remove(kouho);
                        //Debug.Log(blanklist3[blankindex].Count);
                    }
                    else //#そろわず、移動したカードをpreviousMovedCardとして扱い移動する直前のhanduniformsをmotomotoとした
                    {
                        if (previousTurnPlayer == playerNumber) /////comの番のとき
                        {
                            if (info[previousMovedCard] % 13 == blankmod) //引いたカードがブランクだったら
                            {
                                List<List<int>> tmplist = new List<List<int>>();
                                foreach (List<int> kouho in blanklist3[blankindex])
                                {
                                    if (!kouho.Contains(previousMovedCard)) tmplist.Add(kouho);
                                }
                                foreach (List<int> kouho in tmplist) blanklist3[blankindex].Remove(kouho);
                            }
                            else
                            {
                                List<List<int>> tmplist = new List<List<int>>();
                                foreach (List<int> kouho in blanklist3[blankindex])
                                {
                                    if (kouho.Contains(previousMovedCard)) tmplist.Add(kouho);
                                }
                                foreach (List<int> kouho in tmplist) blanklist3[blankindex].Remove(kouho);
                            }
                        }
                        else
                        {
                            List<int> motomoto = handUniforms[previousTurnPlayer];
                            motomoto.Remove(previousMovedCard);
                            List<List<int>> tmplist3 = new List<List<int>>();
                            //Debug.Log(blanklist3[blankindex].Count);
                            foreach (List<int> kouho in blanklist3[blankindex])
                            {
                                foreach (int uniform in motomoto)
                                {
                                    if (kouho.Contains(previousMovedCard) && kouho.Contains(uniform)) tmplist3.Add(kouho);
                                }
                            }
                            foreach (List<int> kouho in tmplist3) blanklist3[blankindex].Remove(kouho);
                            //Debug.Log(blanklist3.Count);
                        }

                    }
                }
            }
        }

        if (blanklist4[blankindex].Count != 0)
        {
            Debug.Log(blanklist4[blankindex].Count + blankindex * 0.1 + playerNumber * 0.01 + 0.004);
        }
        else
        {
            Debug.Log(blanklist3[blankindex].Count + blankindex * 0.1 + playerNumber * 0.01 + 0.003);
            //if (blanklist3[blankindex].Count<5)
            //{
            //    foreach (List<int> a in blanklist3[blankindex])
            //    {
            //        foreach (int b in a) Debug.Log(b);
            //        Debug.Log("\n");
            //    }
            //}
        }
        //if (blanklist4[blankindex].Count<11)
        //{
        //foreach (List<int> a in blanklist4[blankindex])
        //{
        //foreach (int b in a) Debug.Log(b);
        //Debug.Log("\n");
        //}
        //}
    }

    //ここまでブランクziziかく


    private List<int> infoUsingBlank(List<int>[] rec) //ownrecordと違って過去の情報を保存しておく必要はないから毎度読み込むことにした、infoを更新してもいいかも
    {
        List<int> infoub = info;
        if (zizinumber != -1)　//もしかすると引くタイミング的に最新ではないかも
        {
            for (int j = 0; j < blankmods.Count; j++)
            {
                //BlankChaser(record.record, j);　　//ここで回しておかないとblanklist4が１ターンまえの情報になってしまう（実験済）、けどやっぱやめた

                if (blanklist4[j].Count == 1)
                {
                    Debug.Log("ブランク4利用で揃うカード特定");
                    foreach (int blank in blanklist4[j][0])
                    {
                        infoub[blank] = blankmods[j]; //全部同じ数字にしといた
                    }
                }

                if (blanklist4[j].Count > 1)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        blistprivate[j] = Blanklister(blistpublic, j);
                        if (infoub[blistprivate[j][k][0]] == -1 && blistprivate[j][k].Count == 1)
                        {
                            Debug.Log("ブリストプライベイト利用でinfo特定");
                            infoub[blistprivate[j][k][0]] = blankmods[j];
                        }
                    }
                }
            }

            foreach (int i in record.UniformExists)
            {
                if (countN(rec, i) == 1 && i % 13 != zizinumber)
                {
                    Debug.Log("record利用で揃うカード特定");
                    if (infoub[i] != -1) //見た場合に限定してる、見てなくてもペアの判断はできるが実際そのようなことは起こりづらい
                    {
                        for (int j = 0; j < info.Count; j++)
                        {
                            if (rec[i][j] == -1) infoub[j] = infoub[i]; //同じ数字にしといた
                        }
                    }
                }
            }
        }
        return infoub;
    }


    private List<int> success(int drawnPlayer)
    {
        get();
        List<int> suc = new List<int>();
        List<int> info2 = infoUsingBlank(record.record);

        foreach (int un in handUniforms[drawnPlayer])
        {
            if (info2[un] != -1)
            {
                foreach (int myun in handUniforms[playerNumber])
                {
                    if (info2[myun] % 13 == info2[un] % 13)
                    {
                        suc.Add(un);
                        successflag = true;
                    }
                }
            }
        }
        //string debug = playerNumber + "サクセスした！"; ミス
        //Debug.Log(debug);
        //foreach (int b in suc) Debug.Log(b);

        return suc;
    }


    private void Blankzizikaku()
    {
        if (blankmods.Count == 0) return;  //blankindex付いてたのはミス？  
        int debugcnt = 0; //デバッグ用
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
            if (debugcnt > 1) Debug.Log("zizi確してるけど場に３枚ある");  //本当は３枚ある場合も引かないようにしたい
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
        if (!CpuInitialized)
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