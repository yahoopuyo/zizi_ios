using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OpenSourceOnline : MonoBehaviour
{
    GameObject hand;
    HandsOnline hands;
    public List<int> grave;
    private List<GameObject> sources;
    public GameObject cardPrefab;
    public Vector3[] holder;
    public float co;

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
    private void Display()
    {
        initSources();
        //grave.Sort();
        grave = hands.GetGrave();
        foreach (int card in grave)
        {
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();

            sources.Add(cardCopy);

            int num = card % 13;
            int suit = (card - num) / 13;
            cardCopy.transform.position = holder[num] + new Vector3(suit * co, 0f);
            cardModel.cardIndex = card;
            cardModel.ToggleFace(true);
        }
    }
    //private void SetHolder()
    //{
    //    if (holder == null) holder = new Vector3[13];
    //    for(int i = 0; i < 13; i++)
    //    {
    //    holder[i] = new Vector3();
    //    }
    //    for (int i = 1; i < 14; i++)
    //    {
    //        switch (i)
    //        {
    //            case 1:
    //                holder[0] = new Vector3();
    //                break;
    //        }
    //    }
    //}
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hand = GameObject.Find("Hand");
            hands = hand.GetComponent<HandsOnline>();
            Display();
            //grave = hands.GetGrave();
        }

    }
}

