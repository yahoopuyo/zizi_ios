using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public int turn;
    public int turnPlayer;
    public int drawnPlayer;
    public int drawnCard;
    public int preDrawnPlayer;
    public int preDrawnCard;
    InitCanvas init;
    private List<int> Wins = new List<int>();
    public List<string> result = new List<string>();
    private GameObject hand;
    private Hands hands;

    private int CountWinners()
    {
        int winner = 0;
        for(int player = 0; player < 4; player++)
        {
            if (hands.hands[player].Count == 0) winner++;
        }
        return winner;
    }

    public void NextTurnPlayer()
    {
        hand = GameObject.Find("Hand");
        hands = hand.GetComponent<Hands>();
        int nextT = 0;
        switch (CountWinners())
        {
            case 0:
                nextT = (turnPlayer + 1) % 4;
                break;
            case 1:
                if (hands.hands[turnPlayer].Count == 0 || hands.hands[(turnPlayer + 1) % 4].Count == 0) nextT = (turnPlayer + 2) % 4;
                else nextT = (turnPlayer + 1) % 4;
                break;
            case 2:
                if (hands.hands[turnPlayer].Count == 0)
                {
                    if (hands.hands[(turnPlayer + 2) % 4].Count == 0 || hands.hands[(turnPlayer + 1) % 4].Count == 0) nextT = (turnPlayer + 3) % 4;
                    else nextT = (turnPlayer + 2) % 4;
                }
                else
                {
                    if (hands.hands[(turnPlayer + 1) % 4].Count == 0 && hands.hands[(turnPlayer + 2) % 4].Count == 0) nextT = (turnPlayer + 3) % 4;
                    if (hands.hands[(turnPlayer + 1) % 4].Count == 0 && hands.hands[(turnPlayer + 3) % 4].Count == 0) nextT = (turnPlayer + 2) % 4;
                    if (hands.hands[(turnPlayer + 2) % 4].Count == 0 && hands.hands[(turnPlayer + 3) % 4].Count == 0) nextT = (turnPlayer + 1) % 4;
                }
                break;
            default:
                for (int pl = 0; pl < 4; pl++) if (hands.hands[pl].Count != 0) nextT = pl;
                break;
        }
        turnPlayer = nextT;
    }

    public void NextDrawnPlayer()   //常にNextTurnPlayer()が先に呼び出されるようにする必要がある
    {
        int nextD = 0;
        hand = GameObject.Find("Hand");
        hands = hand.GetComponent<Hands>();
        switch (CountWinners())
        {
            case 0:
                nextD = (turnPlayer + 3) % 4;
                break;
            case 1:
                if (hands.hands[(turnPlayer + 3)%4].Count == 0) nextD = (turnPlayer + 2) % 4;
                else nextD = (turnPlayer + 3) % 4;
                break;
            case 2:
                if (hands.hands[(turnPlayer + 3)%4].Count == 0)
                {
                    if(hands.hands[(turnPlayer + 2)%4].Count == 0) nextD = (turnPlayer + 1) % 4;
                    else nextD = (turnPlayer + 2) % 4;
                }
                else nextD = (turnPlayer + 3) % 4;
                break;
            default:
                for (int pl = 0; pl < 4; pl++) if (hands.hands[pl].Count != 0) nextD = pl;
                break;

        }
        preDrawnPlayer = drawnPlayer;
        drawnPlayer = nextD;
    }

    public void turnNext(int cardIndex) //ついでにdrawnCardを更新
    {
        turn++;
        preDrawnCard = drawnCard;
        drawnCard = cardIndex;
        if(CountWinners() > Wins.Count)
        {
            for (int pl = 0; pl < 4; pl++)
            {
                if (hands.hands[pl].Count == 0 && !Wins.Contains(pl))
                {
                    if (pl == 0) result.Add("You were ");
                    else result.Add("Player" + pl + " was ");
                    Wins.Add(pl);
                }
            }
        }
        if (turnPlayer == drawnPlayer)
        {
            int zizi = hands.hands[turnPlayer][0];
            UnityEngine.Debug.Log("zizi is " + (zizi%13 + 1));
            if (turnPlayer == 0) result.Add("You were lost");
            else result.Add("Player" + turnPlayer + " lost");
            init = GetComponent<InitCanvas>();
            init.gameoverP.SetActive(true);
            string Order;
            Order = result[0] + "1st\n\n" + result[1] + "2nd\n\n" + result[2] + "3rd\n\n" + result[3];
            Text text = GameObject.Find("Results").GetComponent<Text>();
            text.text = Order;
        }
    }
    void Start()
    {
        turn = 0;
        turnPlayer = 0;
        drawnPlayer = 3;
        drawnCard = 100;
        preDrawnCard = 100;
        preDrawnPlayer = 100;
    }

    void Update()
    {

    }
}
