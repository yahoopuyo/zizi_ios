using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSlideCPULevel()
    {
        ModeData modeData = GameObject.Find("ModeData").GetComponent<ModeData>();
        Text text = GameObject.Find("SliderText").GetComponent<Text>();
        Slider slider = GameObject.Find("Slider").GetComponent<Slider>();
        int value = (int)slider.value;
        modeData.computerLevel = value;
        text.text = "cpu level ... " + value;
    }
}
