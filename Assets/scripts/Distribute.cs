using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hands))]
public class Distribute : MonoBehaviour
{
    Hands hands;
    public float cardOffset;
    public GameObject cardPrefab;
    private Vector3 start0 = new Vector3(-4f, -3.5f);
    private Vector3 start3 = new Vector3(6, -3.5f);
    private Vector3 start2 = new Vector3(4f, 3.5f);
    private Vector3 start1 = new Vector3(-6f, 3.5f);
    //private Vector3 d_start0 = new Vector3(-3.5f, -1.4f);
    //private Vector3 d_start3 = new Vector3(3.9f, -3.0f);
    //private Vector3 d_start2 = new Vector3(3.5f, 1.4f);
    //private Vector3 d_start1 = new Vector3(-3.9f, 3.5f);
    private Vector3 d_start0 = new Vector3(-3.5f, -1.8f);
    private Vector3 d_start3 = new Vector3(4.3f, -3.0f);
    private Vector3 d_start2 = new Vector3(3.5f, 1.8f);
    private Vector3 d_start1 = new Vector3(-4.3f, 3.5f);
    private List<GameObject> sources;
    void distribute()
    {
        //player0のoriginalsを表示
        int cardCount = 0;
        foreach(int i in hands.Gethand0())
        {
            GameObject cardCopy = Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            Vector3 temp;
            float co = cardOffset * cardCount;

            temp = start0 + new Vector3(co, 0f);
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardModel.ToggleFace(true);

            spriteRenderer.sortingOrder = cardCount;

            cardCount++;
        }
        //player3のoriginalsを表示
        cardCount = 0;
        foreach (int i in hands.Gethand3())
        {
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            Vector3 temp;
            float co = cardOffset * cardCount;

            temp = start3 + new Vector3(0f, co);
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(new Vector3(0f, 0f, 90f));
            cardModel.ToggleFace(false);

            spriteRenderer.sortingOrder = cardCount;

            cardCount++;
        }
        //player2のoriginalsを表示
        cardCount = 0;
        foreach (int i in hands.Gethand2())
        {
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            Vector3 temp;
            float co = cardOffset * cardCount;

            temp = start2 - new Vector3(co, 0f);
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(new Vector3(0f, 0f, 180f));
            cardModel.ToggleFace(false);

            spriteRenderer.sortingOrder = cardCount;

            cardCount++;
        }
        //player1のoriginalを表示
        cardCount = 0;
        foreach (int i in hands.Gethand1())
        {
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            Vector3 temp;
            float co = cardOffset * cardCount;

            temp = start1 - new Vector3(0f, co);
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(new Vector3(0f, 0f, 270f));
            cardModel.ToggleFace(false);

            spriteRenderer.sortingOrder = cardCount;

            cardCount++;
        }

        //player0のdrawnsを表示
        cardCount = 0;
        foreach (int i in hands.drawns[0])
        {
            GameObject cardCopy = Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            Vector3 temp;
            float co = cardOffset * cardCount;

            temp = d_start0 + new Vector3(co, 0f)*1.7f;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardModel.ToggleFace(true);

            spriteRenderer.sortingOrder = cardCount;

            cardCount++;
        }
        //player3のdrawnsを表示
        cardCount = 0;
        foreach (int i in hands.drawns[3])
        {
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            Vector3 temp;
            float co = cardOffset * cardCount;

            temp = d_start3 + new Vector3(0f, co)*1.7f;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(new Vector3(0f, 0f, 90f));
            cardModel.ToggleFace(false);

            spriteRenderer.sortingOrder = cardCount;

            cardCount++;
        }
        //player2のdrawnsを表示
        cardCount = 0;
        foreach (int i in hands.drawns[2])
        {
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            Vector3 temp;
            float co = cardOffset * cardCount;

            temp = d_start2 - new Vector3(co, 0f)*1.7f;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(new Vector3(0f, 0f, 180f));
            cardModel.ToggleFace(false);

            spriteRenderer.sortingOrder = cardCount;

            cardCount++;
        }
        //player1のdrawnsを表示
        cardCount = 0;
        foreach (int i in hands.drawns[1])
        {
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            Vector3 temp;
            float co = cardOffset * cardCount;

            temp = d_start1 - new Vector3(0f, co)*1.7f;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(new Vector3(0f, 0f, 270f));
            cardModel.ToggleFace(false);

            spriteRenderer.sortingOrder = cardCount;

            cardCount++;
        }

    }

    private void initSources()
    {
        if (sources == null) sources = new List<GameObject>();
        else
        {
            foreach (GameObject source in sources)
            {
                Destroy(source); //今表示しているgameobjectのを消す。
            }
            sources.Clear();

        }
    }

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(120, 10, 100, 20), "distribute"))
    //    {
    //        initSources();
    //        hands = GetComponent<Hands>();
    //        distribute();
    //        Center();
    //        foreach (GameObject source in sources)
    //        {
    //            source.name = "Card" + source.GetComponent<CardModel>().cardIndex;
    //        }
    //    }
    //}

    public void StartGame()
    {
        initSources();
        hands = GetComponent<Hands>();
        distribute();
        Center();
        foreach (GameObject source in sources)
        {
            source.name = "Card" + source.GetComponent<CardModel>().cardIndex;
        }
    }
    public void Center()
    {
        List<int> grave = hands.GetGrave();
        GameObject cardCopy = Instantiate(cardPrefab);
        CardModel cardModel = cardCopy.GetComponent<CardModel>();

        cardModel.cardIndex = grave[grave.Count - 1];
        cardModel.ToggleFace(true);

        cardCopy.transform.position = new Vector3(0,0,0);
        sources.Add(cardCopy);
    }

    public void updateField()
    {
        initSources();
        hands = GetComponent<Hands>();
        distribute();
        Center();
        foreach(GameObject source in sources)
        {
            source.name = "Card" + source.GetComponent<CardModel>().cardIndex;
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
