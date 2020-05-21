using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordOnline : MonoBehaviour
{
    public List<int> Uniform;
    public List<int>[] record;
    public List<int> UniformExists;
    public List<int>[] info = new List<int>[4];
    public int RecordSize = 0;
    private List<int>[] handUniform = new List<int>[4];
    public bool Initialized = false;
    

    GameObject Hand;
    HandsOnline hands;
    ZiziDeck deck;

    private void get()
    {
        Hand = GameObject.Find("Hand");
        hands = Hand.GetComponent<HandsOnline>();
    }
    private void FirstRecord(int size)
    {
        record = new List<int>[size];
        for (int i = 0; i < size; i++)
        {
            if (record[i] == null) record[i] = new List<int>();
            for (int j = 0; j < size; j++) record[i].Add(-1);
        }
    }

    private void Initialize()
    {
        get();
        InitRecord();
        InitInfo();
        InitUniforms();
        InitUniformExists();
    }

    private void InitUniforms()
    {
        for (int i = 0; i < 4; i++) if (handUniform[i] == null) handUniform[i] = new List<int>();
        if (Uniform == null) Uniform = new List<int>();
        for (int pn = 0; pn < 4; pn++)
        {
            foreach (int card in hands.originals[pn])
            {
                Uniform.Add(card);
            }
        }
    }
    private void InitUniformExists()
    {
        if (UniformExists == null) UniformExists = new List<int>();
        for (int pn = 0; pn < RecordSize; pn++) UniformExists.Add(pn);
    }
    private void InitInfo()
    {
        for (int i = 0; i < 4; i++) if (info[i] == null) info[i] = new List<int>();
        for (int pn = 0; pn < 4; pn++)
        {
            foreach (int card in hands.originals[pn])
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == pn) info[i].Add(card);
                    else info[i].Add(-1);
                }
            }
        }
    }
    private void InitRecord()
    {
        List<int> numList = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            numList.Add(hands.originals[i].Count);
            RecordSize += hands.originals[i].Count;
        }
        //Debug.Log(RecordSize);
        FirstRecord(RecordSize);
        int tmp = 0;
        foreach (int i in numList)
        {
            for (int m = tmp; m < tmp + i; m++) for (int j = tmp; j < tmp + i; j++) record[m][j] = 0;
            tmp += i;
        }
    }

    public void DebugRecords()
    {
        for (int i = 0; i < RecordSize; i++)
        {
            foreach (int k in record[i])
            {
                //Debug.Log(k);
            }
        }
        //for (int pn = 0; pn < 4; pn++) foreach (int card in info[pn]) Debug.Log(card);
    }

    public void replaceRecord(int x, int y, int value)
    {
        if (record[x][y] == -1) record[x][y] = value;
        if (record[y][x] == -1) record[y][x] = value;
    }

    public void updateRecordUnpaired(int turn, int drawnCard, int tP)
    {
        get();
        List<int> tPUniforms = new List<int>();
        int drawnUniform = Uniform.IndexOf(drawnCard);
        foreach (int card in hands.hands[tP]) tPUniforms.Add(Uniform.IndexOf(card));
        foreach (int num in tPUniforms) replaceRecord(drawnUniform, num, turn);
    }

    public void updateRecordPaired(int turn, int drawnCard, int pairCard)
    {
        int drawnUniform = Uniform.IndexOf(drawnCard);
        int pairUniform = Uniform.IndexOf(pairCard);
        UniformExists.Remove(drawnUniform);
        UniformExists.Remove(pairUniform);
        for (int i = 0; i < RecordSize; i++)
        {
            replaceRecord(i, drawnUniform, turn);
            replaceRecord(i, pairUniform, turn);
        }
    }

    public void updateInfoUnpaired(int tP, int drawnCard)
    {
        int drawnUniform = Uniform.IndexOf(drawnCard);
        info[tP][drawnUniform] = drawnCard;
    }

    public void updateInfoPaired(int drawnCard, int pairedCard)
    {
        int drawnUniform = Uniform.IndexOf(drawnCard);
        int pairedUniform = Uniform.IndexOf(pairedCard);

        for (int i = 0; i < 4; i++)
        {
            info[i][drawnUniform] = drawnCard;
            info[i][pairedUniform] = pairedCard;
        }
    }
    public List<int>[] GetHandUniform()
    {
        get();
        handUniform = new List<int>[4];
        for (int i = 0; i < 4; i++) handUniform[i] = new List<int>();
        for (int pn = 0; pn < 4; pn++)
        {
            foreach (int card in hands.hands[pn])
            {
                handUniform[pn].Add(Uniform.IndexOf(card));
            }
        }
        return handUniform;
    }

    public List<int>[] GetOriginalUniform()
    {
        get();
        List<int>[] originalUniform = new List<int>[4];
        for (int i = 0; i < 4; i++) originalUniform[i] = new List<int>();
        for (int pn = 0; pn < 4; pn++)
        {
            foreach (int card in hands.originals[pn])
            {
                originalUniform[pn].Add(Uniform.IndexOf(card));
            }
        }
        return originalUniform;
    }

    public List<int>[] GetDrawnUniform()
    {
        get();
        List<int>[] drawnUniform = new List<int>[4];
        for (int i = 0; i < 4; i++) drawnUniform[i] = new List<int>();
        for (int pn = 0; pn < 4; pn++)
        {
            foreach (int card in hands.drawns[pn])
            {
                drawnUniform[pn].Add(Uniform.IndexOf(card));
            }
        }
        return drawnUniform;
    }

    public List<int> opensource()
    {
        get();
        return hands.GetGrave();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Initialized)
        {
            get();
            if(hands.distributed)
            {
                Initialize();
                //Debug.Log(RecordSize);
                //DebugRecords();
                Initialized = true;
            }
        }
    }
}
