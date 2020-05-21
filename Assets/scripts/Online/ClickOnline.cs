using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon;
using System;

public class ClickOnline : Photon.MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitH​​andler
{
    private GameObject hand;
    private GameObject gameManager;
    DrawOnline draw;
    ZiziKakuOnline zizikaku;
    private TurnManagerOnline turnManager;
    private HandsOnline hands;
    private DistributeForAll distribute;
    CardModel cardModel;
    int preDrawnPlayer;
    int drawnCard;
    int cardIndex;
    int turnPlayer;
    int drawnPlayer;
    int player;

    void get()
    {
        gameManager = GameObject.Find("GameManager");
        draw = gameManager.GetComponent<DrawOnline>();
        turnManager = gameManager.GetComponent<TurnManagerOnline>();
        turnPlayer = turnManager.turnPlayer;
        drawnPlayer = turnManager.drawnPlayer;
        preDrawnPlayer = turnManager.preDrawnPlayer;
        drawnCard = turnManager.drawnCard;
        hand = GameObject.Find("Hand"); //Handのクラスを取得
        hands = hand.GetComponent<HandsOnline>();
        cardModel = GetComponent<CardModel>();
        cardIndex = cardModel.cardIndex; //カードモデルからcardIndexを取得
        zizikaku = gameManager.GetComponent<ZiziKakuOnline>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        get();

        if (drawnCard == cardIndex && preDrawnPlayer == player) //前ひかれたカードだったら
        {
            List<int> grave = hands.GetGrave();
            int l = grave.Count;
            if (drawnCard != hands.GetGrave()[l - 1] && drawnCard != hands.GetGrave()[l - 2]) //真ん中のカードじゃなかったら
            {
                cardModel.ToggleFace(true);
                Debug.Log("selected");
            }
        }
        if (drawnPlayer == hands.Cardownerreturn(cardIndex))
        {
            var v = new Vector2(0, 0.4f);
            transform.Translate(v);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        get();
        if (drawnCard == cardIndex && preDrawnPlayer == player) //前ひかれたカードだったら
        {
            List<int> grave = hands.GetGrave();
            int l = grave.Count;
            if (drawnCard != hands.GetGrave()[l-1] && drawnCard != hands.GetGrave()[l-2]) //真ん中のカードじゃなかったら
            {
                cardModel.ToggleFace(false);
                Debug.Log("selected");
            }
        }
        if (drawnPlayer == hands.Cardownerreturn(cardIndex))
        {
            var v = new Vector2(0, -0.4f);
            transform.Translate(v);
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        get();
        if (eventData.button == PointerEventData.InputButton.Left) //クリック回数>0の時  一応残すけどこのままいくならif文とっても良いはず
        {
            if (turnPlayer != player) return;
            if (drawnPlayer == hands.Cardownerreturn(cardIndex))
            {
                //hands.hands[drawnPlayer].Remove(cardIndex); //引かれる人の手札配列からカードを削除
                //hands.hands[turnPlayer].Add(cardIndex); //引いた人の手札配列にカードを追加
                //hands.DeletePair((cardIndex % 13) + 1,turnPlayer);
                //hands.ClickUpdate();
                //distribute = hand.GetComponent<Distribute>();
                //distribute.updateField();
                //turnManager.NextTurnPlayer();
                //turnManager.NextDrawnPlayer();
                //turnManager.turnNext();
                if (draw.moveFlag || draw.flashFlag) return;
                draw.drawWithAnimation(drawnPlayer, cardIndex, turnPlayer);
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            int owner = hands.Cardownerreturn(cardIndex);
            bool face;
            if (owner == 5) return; //選ばれたカードが誰のものでもなかったら
            if (zizikaku.UpdateGuessList(cardIndex)) return;  //もう6つ選択していて、増やそうとしている場合
            cardModel.ToggleZizikaku();
            cardModel.ToggleFace(owner == player);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ModeData").GetComponent<ModeData>().player;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
