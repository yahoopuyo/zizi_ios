using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ZiziDeck))]
public class DeckView : MonoBehaviour
{
    private int player = 4;
    ZiziDeck deck;
    public Vector3 start1,start2,start3,start4;  //最初のカードの位置
    public float cardOffset;　//カードをずらす幅
    public GameObject cardPrefab; //instantiateするプレファブ

    private void Start()
    {
        deck = GetComponent<ZiziDeck>();　//Deck.csの取得
        DistributeCards();　//下記メソッドの実行
    }

    void DistributeCards()　//メソッド本体
    {
        int cardCount = 0; //内部で使う値cardCountの宣言

        foreach (int i in deck.GetCards())
        {
            float co = cardOffset * (cardCount / player); //オフセット幅の計算
            Vector3 temp;
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            //Transform transform = cardCopy.GetComponent<Transform>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            switch (cardCount % player)
            {
                case 0:
                    temp = start1 + new Vector3(co, 0f);
                    //tempというオフセットした位置の計算
                    cardCopy.transform.position = temp;
                    //現在の位置にtempを代入
                    cardCopy.transform.Rotate(new Vector3(0f, 0f, 90f));
                    cardModel.cardIndex = i;
                    cardModel.ToggleFace(true);

                    spriteRenderer.sortingOrder = cardModel.faces.Length - cardCount;
                    break;

                case 1:
                    temp = start2 + new Vector3(0f, co);
                    //tempというオフセットした位置の計算
                    cardCopy.transform.position = temp;
                    //現在の位置にtempを代入
                    cardModel.cardIndex = i;
                    cardModel.ToggleFace(true);

                    spriteRenderer.sortingOrder = cardModel.faces.Length - cardCount;
                    break;
                case 2:
                    temp = start3 - new Vector3(co, 0f);
                    //tempというオフセットした位置の計算
                    cardCopy.transform.position = temp;
                    //現在の位置にtempを代入
                    cardCopy.transform.Rotate(new Vector3(0f, 0f, 270f));
                    cardModel.cardIndex = i;
                    cardModel.ToggleFace(true);

                    spriteRenderer.sortingOrder = cardModel.faces.Length - cardCount;
                    break;
                case 3:
                    temp = start4 - new Vector3(0f, co);
                    //tempというオフセットした位置の計算
                    cardCopy.transform.position = temp;
                    //現在の位置にtempを代入
                    cardModel.cardIndex = i;
                    cardModel.ToggleFace(true);

                    spriteRenderer.sortingOrder = cardModel.faces.Length - cardCount;
                    break;
            }



            cardCount++; //cardCountをインクリメント

        }
    }



}
