using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugKifu : MonoBehaviour
{
    Record record;
    Text recordtxt;
    public void ShowRecord()
    {
        recordtxt = GetComponent<Text>();
        record = GameObject.Find("GameManager").GetComponent<Record>();
        string tmp = "";
        //tmp=string.Join(' ',record.record[0])
        for(int i = 0; i < record.RecordSize; i++)
        {
            foreach(int k in record.record[i])
            {
                if (k == -1 || k > 9) tmp = tmp + k + ' ';
                else tmp = tmp + k + ' ' + ' ';
            }
            tmp = tmp + '\n';
        }
        for(int i = 0; i < 4; i++)
        {
            foreach(int card in record.info[i])
            {
                if (card == -1 || card > 9) tmp = tmp + card + ' ';
                else tmp = tmp + card + ' ' + ' ';
            }
            tmp = tmp + '\n';
        }
        recordtxt.text = tmp;
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
