using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeCard : MonoBehaviour
{
    private CardModel cardModel;
    private CardFlipper flipper;
    int cardIndex = 0;
    int backIndex = 0;

    public GameObject card;

    void Awake()
    {
        cardModel = card.GetComponent<CardModel>();
        flipper = card.GetComponent<CardFlipper>();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 20), "Hit me")) //location 10,10 size 100*20
        {

            if (cardIndex == cardModel.faces.Length)
            {
                cardIndex = 0;
                cardModel.cardIndex = cardIndex;
                cardModel.ToggleFace(true);
            }
            else
            {
                cardModel.cardIndex = cardIndex;
                cardModel.ToggleFace(true);
            }
            cardIndex++;

        }
        if (GUI.Button(new Rect(120, 10, 100, 20), "Change back"))
        {
            if (backIndex == cardModel.cardBack.Length)
            {
                backIndex = 0;
                cardModel.backIndex = backIndex;
                cardModel.ToggleFace(false);
            }
            else
            {
                cardModel.backIndex = backIndex;
                cardModel.ToggleFace(false);
            }
            backIndex++;
        }
        if (GUI.Button(new Rect(230, 10, 100, 20), "flip card"))
        {
            if (cardIndex >= cardModel.faces.Length)
            {
                cardIndex = 0;
                flipper.flipCard(cardModel.faces[cardModel.faces.Length - 1], cardModel.cardBack[backIndex], -1);
            }
            else
            {
                if(cardIndex > 0)
                {
                    flipper.flipCard(cardModel.faces[cardIndex - 1], cardModel.faces[cardIndex], cardIndex);
                }
                else
                {
                    flipper.flipCard(cardModel.cardBack[backIndex], cardModel.faces[cardIndex], cardIndex);
                }
                cardIndex++;
            }
        }
    }
}
