using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class DistributeForAll : MonoBehaviour
{
    public HandsOnline hands;
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

    private List<Vector3> start = new List<Vector3>();
    private List<Vector3> d_start = new List<Vector3>();
    private List<GameObject> sources;
    private List<Vector3> rotate = new List<Vector3>();
    private List<Vector3> ofset = new List<Vector3>();

    private int player;
    ModeData md;
    ZiziKakuOnline zizikaku;

    public void distribute()
    {

    //player0のoriginalsを表示
    int cardCount = 0;
        foreach (int i in hands.Gethand0())
        {
            Vector3 temp;
            //float co = cardOffset * cardCount;
            temp = start[0] + ofset[0]*cardCount ;
            //GameObject cardCopy = PhotonNetwork.Instantiate("Card",temp,new Quaternion(),0);
            GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();


            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;
            cardCopy.transform.Rotate(rotate[0]);

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            if (player == 0) cardModel.ToggleFace(true);
            else cardModel.ToggleFace(false);
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
            //float co = cardOffset * cardCount;

            temp = start[3] + ofset[3]*cardCount ;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(rotate[3]);
            if(player ==3) cardModel.ToggleFace(true);
            else cardModel.ToggleFace(false);
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
            //float co = cardOffset * cardCount;

            temp = start[2] + ofset[2]*cardCount ;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(rotate[2]);
            if(player == 2) cardModel.ToggleFace(true);
            else cardModel.ToggleFace(false);

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
            //float co = cardOffset * cardCount;

            temp = start[1] + ofset[1]*cardCount ;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(rotate[1]);
            if(player == 1) cardModel.ToggleFace(true);
            else cardModel.ToggleFace(false);

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
            //float co = cardOffset * cardCount;

            temp = d_start[0] + ofset[0]*cardCount  * 1.7f;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;
            cardCopy.transform.Rotate(rotate[0]);

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            if(player ==0) cardModel.ToggleFace(true);
            else cardModel.ToggleFace(false);

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
            //float co = cardOffset * cardCount;

            temp = d_start[3] + ofset[3]*cardCount  * 1.7f;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(rotate[3]);
            if(player ==3) cardModel.ToggleFace(true);
            else cardModel.ToggleFace(false);

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
            //float co = cardOffset * cardCount;

            temp = d_start[2] + ofset[2]*cardCount  * 1.7f;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(rotate[2]);
            if(player ==2)cardModel.ToggleFace(true);
            else cardModel.ToggleFace(false);

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
            //float co = cardOffset * cardCount;

            temp = d_start[1] + ofset[1]*cardCount  * 1.7f;
            cardModel.backIndex = hands.GetBack(i);
            cardModel.cardIndex = i;

            sources.Add(cardCopy);

            cardCopy.transform.position = temp;
            cardCopy.transform.Rotate(rotate[1]);
            if (player == 1) cardModel.ToggleFace(true);
            else cardModel.ToggleFace(false);

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
        hands = GetComponent<HandsOnline>();
        distribute();
        Center();
        foreach (GameObject source in sources)
        {
            source.name = "Card" + source.GetComponent<CardModel>().cardIndex;
        }
        zizikaku = GameObject.Find("GameManager").GetComponent<ZiziKakuOnline>();
    }
    public void Center()
    {
        List<int> grave = hands.GetGrave();
        GameObject cardCopy = Instantiate(cardPrefab);
        CardModel cardModel = cardCopy.GetComponent<CardModel>();

        cardModel.cardIndex = grave[grave.Count - 1];
        cardModel.ToggleFace(true);

        cardCopy.transform.position = new Vector3(0, 0, 0);
        sources.Add(cardCopy);
    }

    public void updateField()
    {
        initSources();
        hands = GetComponent<HandsOnline>();
        distribute();
        Center();
        foreach (GameObject source in sources)
        {
            CardModel cm = source.GetComponent<CardModel>();
            int index = cm.cardIndex;
            source.name = "Card" + index;
            if (zizikaku.InGuessList(index))
            {
                cm.ToggleZizikaku();
                cm.ToggleFace(true);
            }

        }
    }

    private void ShiftList(List<Vector3> list)
    {
        List<Vector3> tmp = new List<Vector3>(list);
        for(int i = 0; i < 4; i++)
        {
            list[i] = tmp[(4 - player + i) % 4];
        }
    }

    // Start is called before the first frame update
    public void SetVectors()
    {
        start.Add(start0);
        start.Add(start1);
        start.Add(start2);
        start.Add(start3);

        d_start.Add(d_start0);
        d_start.Add(d_start1);
        d_start.Add(d_start2);
        d_start.Add(d_start3);

        ofset.Add(new Vector3(cardOffset, 0f));
        ofset.Add(new Vector3(0f, -cardOffset));
        ofset.Add(new Vector3(-cardOffset,0));
        ofset.Add(new Vector3(0f,cardOffset));

        rotate.Add(new Vector3(0f, 0f, 0f));
        rotate.Add(new Vector3(0f, 0f, 270f));
        rotate.Add(new Vector3(0f, 0f, 180f));
        rotate.Add(new Vector3(0f, 0f, 90f));

        //PhotonView view = GetComponent<PhotonView>();
        md = GameObject.Find("ModeData").GetComponent<ModeData>();
        player = md.player;

        ShiftList(start);
        ShiftList(d_start);
        ShiftList(ofset);
        ShiftList(rotate);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
