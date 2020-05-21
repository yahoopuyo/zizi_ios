using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ZiziDeck))]
public class HandsOnline : MonoBehaviour
{
    public ZiziDeck deck;
    private bool IsEasy = true;
    public bool distributed=false;
    public List<int>[] hands;
    public List<int>[] originals;
    public List<int>[] drawns;
    public List<int> originalBack;
    private List<int> grave;
    private int k = 0;

    public List<int>[] Gethands()
    {
        return originals;
    }
    public List<int> Gethand0()
    {
        return originals[0];
    }
    public List<int> Gethand1()
    {
        return originals[1];
    }
    public List<int> Gethand2()
    {
        return originals[2];
    }
    public List<int> Gethand3()
    {
        return originals[3];
    }

    public List<int> GetGrave()
    {
        return grave;
    }

    public void DeletePair(int num, int player)  //num is 1~13
    {
        int count = 0;
        int removed = 0;
        int[] numbers = { num - 1, num + 12, num + 25, num + 38 };
        foreach (int card in hands[player])
        {
            if (card % 13 == num - 1) count++;
        }
        foreach (int i in numbers)
        {
            if (removed < 2 && count > 1)
            {
                if (hands[player].Remove(i))
                {
                    grave.Add(i);
                    removed++;
                }
            }
        }

    }

    public int FindDeletedPair(int drawnCard, int turnPlayer)//揃わなかったら100,揃ったら揃ったカード
    {
        int deletedPair = 100;
        foreach (int cI in hands[turnPlayer])
        {
            if (cI % 13 == drawnCard % 13) deletedPair = cI;
        }
        return deletedPair;
    }

    private void InitBackList()
    {
        originalBack = new List<int>();
        for (int cardIndex = 0; cardIndex < 52; cardIndex++)
        {
            for (int i = 0; i < 4; i++) if (hands[i].Contains(cardIndex)) originalBack.Add(i);
            if (cardIndex == deck.GetZizi()) originalBack.Add(4);
        }
    }

    public int GetBack(int card)
    {
        if (IsEasy) return originalBack[card];
        else return 2;
    }


    public int Cardownerreturn(int index) //カードの持ち主を返す関数
    {
        int ans = 5;
        for (int player = 0; player < 4; player++)
        {
            foreach (int card in hands[player])
            {
                if (card == index) ans = player;
            }
        }
        return ans;
    }

    public void Delete()
    {
        for (int players = 0; players < 4; players++)
        {
            for (int a = 1; a < 14; a++) DeletePair(a, players);
        }
    }
    void CardList()
    {
        if (hands[0] == null) hands[0] = new List<int>();
        else hands[0].Clear();
        if (hands[1] == null) hands[1] = new List<int>();
        else hands[1].Clear();
        if (hands[2] == null) hands[2] = new List<int>();
        else hands[2].Clear();
        if (hands[3] == null) hands[3] = new List<int>();
        else hands[3].Clear();

        foreach (int l in deck.GetCards())
        {
            if (k >= 0 && k < 13) hands[0].Add(l);
            else if (k > 12 && k < 26) hands[1].Add(l);
            else if (k > 25 && k < 39) hands[2].Add(l);
            else hands[3].Add(l);
            k++;
        }
    }

    void makeoriginals()
    {
        for (int i = 0; i < 4; i++) //player0~3
        {
            if (originals[i] == null) originals[i] = new List<int>();
            else originals[i].Clear();
            foreach (int card in hands[i])
            {
                originals[i].Add(card);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        ModeData modeData = GameObject.Find("ModeData").GetComponent<ModeData>();
        IsEasy = modeData.IsEasy();
        deck = GetComponent<ZiziDeck>();
        /*
        hands = new List<int>[4];
        originals = new List<int>[4];
        drawns = new List<int>[4];
        for (int i = 0; i < 4; i++)
        {
            if (drawns[i] == null) drawns[i] = new List<int>();
            else drawns[i].Clear();
        }
        grave = new List<int>();
        CardList();
        InitBackList(); //orginalBack[cardIndex]でbackIndexを返す、ziziは4
        Delete();
        Delete();
        makeoriginals(); //originals配列を作成
                             //Record record = GameObject.Find("GameManager").GetComponent<Record>();
                             //record.InitRecord(originals);
                             //record.DebugRecord();
       */
        
    }

    private void Update()
    {
        //deck = GetComponent<ZiziDeck>();

        if (deck.shared && !distributed)
        {
            hands = new List<int>[4];
            originals = new List<int>[4];
            drawns = new List<int>[4];

            for (int i = 0; i < 4; i++)
            {
                if (drawns[i] == null) drawns[i] = new List<int>();
                else drawns[i].Clear();
            }
            grave = new List<int>();
            CardList();
            InitBackList(); //orginalBack[cardIndex]でbackIndexを返す、ziziは4
            Delete();
            Delete();
            makeoriginals(); //originals配列を作成
            GetComponent<DistributeForAll>().SetVectors();
            GetComponent<DistributeForAll>().StartGame();
            distributed = true;
        }
    }

    public void ClickUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            //drawnsへの追加
            foreach (int card in hands[i])
            {
                int local = 0;
                if (originals[i] != null)
                {
                    foreach (int o_card in originals[i])
                    {
                        if (card == o_card) local++;
                    }
                }
                if (drawns[i] != null)
                {
                    foreach (int d_card in drawns[i])
                    {
                        if (card == d_card) local++;
                    }
                }
                if (local == 0)
                {
                    drawns[i].Add(card);
                }
            }

            //drawnsからの削除
            int del_num = 100;
            foreach (int d_card in drawns[i])
            {
                int local = 0;
                foreach (int card in hands[i])
                {
                    if (d_card == card) local++;
                }

                if (local == 0) del_num = d_card;

            }
            if (del_num != 100) drawns[i].Remove(del_num);


            del_num = 100;
            //originalsからの削除
            foreach (int o_card in originals[i])
            {
                int local = 0;
                foreach (int card in hands[i])
                {
                    if (o_card == card)
                    {
                        local++;
                    }
                }

                if (local == 0) del_num = o_card;
            }
            if (del_num != 100) originals[i].Remove(del_num);
        }
    }


}

