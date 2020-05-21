using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.UI;

public class CardModel : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Sprite[] faces;
    public Sprite[] cardBack;

    public int cardIndex; //faces[cardIndex]
    public int backIndex;  //cardBack[Original]

    private bool zizikaku_selected;

    public bool debug;  //if true, show the face of all the cards
    public void ToggleFace(bool showFace)
    {
        if (zizikaku_selected)  //chosen as zizi
        {
            spriteRenderer.sprite = cardBack[0];
        }
        else
        {
            if (showFace | debug)
            {
                spriteRenderer.sprite = faces[cardIndex];
            }
            else
            {
                spriteRenderer.sprite = cardBack[backIndex];
            }
        }   
    }
    public void ToggleZizikaku()
    {
        zizikaku_selected = !zizikaku_selected;
    }

    public void SetZizikaku(bool isZizi)
    {
        zizikaku_selected = isZizi;
    }
    public bool GetZizikaku()
    {
        return zizikaku_selected;
    }
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
