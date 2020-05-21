using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public bool flag = true;   //プレイヤーが一人のときは、true
    public bool moveFlag = false; //処理待機(waitforsecond)中はtrue
    public bool flashFlag = false; //処理待機(waitforsecond)中はtrue
    string cardName;
    private int turn;
    private int tP;
    private int dP;
    private int level;
    GameObject hand;
    GameObject card;
    ComputerVer2[] coms = new ComputerVer2[3];
    Hands hands;
    TurnManager turnManager;
    Record record;
    Distribute distribute;
    public bool which;

    private void get()
    {
        //card = GameObject.Find("Card");
        turnManager = GetComponent<TurnManager>();
        record = GetComponent<Record>();
        turn = turnManager.turn;
        tP = turnManager.turnPlayer;
        dP = turnManager.drawnPlayer;
        hand = GameObject.Find("Hand"); //Handのクラスを取得
        hands = hand.GetComponent<Hands>();
        for (int i = 0; i < 3; i++)
        {
            coms[i] = GameObject.Find("Com" + (i + 1)).GetComponent<ComputerVer2>();
            //Debug.Log("Com" + (i + 1)); 
        }
    }

    IEnumerator DrawWithAnimation(int drawnPlayer,int cardIndex,int turnPlayer)
    {
        get();
        cardName = "Card" + cardIndex;
        card = GameObject.Find(cardName);
        moveFlag = true;
        yield return new WaitForSeconds(0.5f);  //0.5秒待機
        moveFlag = false;
        int deleted = hands.FindDeletedPair(cardIndex, turnPlayer);
        if (deleted != 100)
        {
            flashFlag = true;   //アニメーション最中のアクション無効化用

            cardName = "Card" + deleted;
            card = GameObject.Find(cardName);
            var color = card.GetComponent<SpriteRenderer>().color;
            color.a = 0;
            card.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.2f);
            color.a = 1f;
            card.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.2f);
            color.a = 0;
            card.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.2f);
            color.a = 1f;
            card.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(1f);

            record.updateRecordPaired(turn + 1, cardIndex, deleted);    //棋譜操作
            record.updateInfoPaired(cardIndex, deleted);    //プライベート情報操作
            flashFlag = false;
        }
        else
        {
            record.updateRecordUnpaired(turn + 1, cardIndex, turnPlayer);  //棋譜操作
            record.updateInfoUnpaired(turnPlayer, cardIndex);   //プライベート情報操作
        }
        
        hands.hands[dP].Remove(cardIndex); //引かれる人の手札配列からカードを削除
        hands.hands[tP].Add(cardIndex); //引いた人の手札配列にカードを追加
        hands.DeletePair((cardIndex % 13) + 1, turnPlayer);
        hands.ClickUpdate();
        distribute = hand.GetComponent<Distribute>();
        distribute.updateField();
        turnManager.NextTurnPlayer();
        turnManager.NextDrawnPlayer();
        turnManager.turnNext(cardIndex);
        int deletedUniform;
        if (deleted == 100) deletedUniform = -1;
        else deletedUniform = record.Uniform.IndexOf(deleted);
        for (int cn = 0; cn < 3; cn++)
        {
            coms[cn].load(dP, record.Uniform.IndexOf(cardIndex), tP,deletedUniform);
        }
    }

    public void drawWithAnimation(int drawnPlayer,int cardIndex,int turnPlayer)
    {
        StartCoroutine(DrawWithAnimation(drawnPlayer,cardIndex,turnPlayer));
        StopCoroutine(DrawWithAnimation(drawnPlayer, cardIndex,turnPlayer));

    }

    private int draw(int drawnPlayer)
    {
        if (level == 1 || level == 2) return cpu1(drawnPlayer);
        else return cpu2(drawnPlayer);
    }

    private int cpu1(int drawnPlayer)
    {
        int index = Random.Range(0, hands.hands[drawnPlayer].Count);
        int cI = hands.hands[drawnPlayer][index];
        return cI;
    }

    private int cpu2(int drawnPlayer)
    {
        int rand = Random.Range(0,2);
        Debug.Log(rand);
        if (hands.drawns[drawnPlayer].Count == 0 || hands.originals[drawnPlayer].Count == 0) return cpu1(drawnPlayer);
        if (rand == 0) return fromOriginal(drawnPlayer);
        if (rand == 1) return fromDrawns(drawnPlayer);
        return hands.hands[drawnPlayer][0];
    }

    private int fromOriginal(int drawnPlayer)
    {
        int index = Random.Range(0, hands.originals[drawnPlayer].Count);
        int cI = hands.originals[drawnPlayer][index];
        return cI;
    }

    private int fromDrawns(int drawnPlayer)
    {
        int index = Random.Range(0, hands.drawns[drawnPlayer].Count);
        int cI = hands.drawns[drawnPlayer][index];
        return cI;
    }

    void Start()
    {
        ModeData modeData = GameObject.Find("ModeData").GetComponent<ModeData>();
        flag = modeData.GetComponent<ModeData>().IsSolo(); //　的な感じ？
        level = modeData.computerLevel;
    }

    //void OnGUI()
    //{
    //    if (flag)
    //    {
    //        if (GUI.Button(new Rect(300, 10, 100, 20), "CPU turn draw"))
    //        {
    //            get();
    //            if (tP != 0)
    //            {
    //                if (moveFlag || flashFlag) return;  //待機処理中にもう一回押された時に無効化
    //                drawWithAnimation(dP,draw(dP),tP);
    //            }
    //        }
    //    }
    //}


    // Update is called once per frame
    void Update()
    {
        if (moveFlag)
        {
            card.transform.Translate(0, Time.deltaTime * 0.6f, 0);
        }

        if (flag)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                get();
                if (tP != 0)
                {
                    if (moveFlag || flashFlag) return;  //待機処理中にもう一回押された時に無効化
                    int drawnUniform;
                    if (level == 1) drawnUniform = cpu1(dP);
                    else if (level == 2) drawnUniform = cpu2(dP);
                    else drawnUniform = record.Uniform[coms[tP - 1].draw(dP)];
                    
                    drawWithAnimation(dP, drawnUniform, tP);
                }
            }
        }


        //if (tP == 0)
        //{
        //    card.GetComponent<Click>().enabled = true;
        //    which = true;
        //}
        //else
        //{
        //card.GetComponent<Click>().enabled = false;
        //which = false;
        //}
        //↑これでできると思ったんだけど、なぜかできないので、clickを少しだけ動かした。
    }
}
