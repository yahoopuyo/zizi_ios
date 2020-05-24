using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCanvas : MonoBehaviour
{
    GameObject canvasStart;
    GameObject setP;
    GameObject setB;
    GameObject debugP;
    public GameObject gameoverP;
    [SerializeField] public GameObject  comDrawbtn;
    private ModeData md;
    // Start is called before the first frame update
    void Start()
    {
        md = GameObject.Find("ModeData").GetComponent<ModeData>();
        if(!md.IsHost() || md.IsSolo()) comDrawbtn.SetActive(false);
        canvasStart = GameObject.Find("Canvas_0");
        canvasStart.SetActive(true);
        setP = GameObject.Find("SettingPanel");
        setP.SetActive(false);
        setB = GameObject.Find("SettingButton");
        setB.SetActive(true);
        gameoverP = GameObject.Find("GameOverPanel");
        gameoverP.SetActive(false);
        debugP = GameObject.Find("debugPanel");
        debugP.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
