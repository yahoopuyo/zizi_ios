using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public bool backmenu;
    public void OnClick()
    {
        //ソロかオンラインか
        if (GameObject.Find("ModeData").GetComponent<ModeData>().IsSolo())
        {
            Destroy(GameObject.Find("ModeData"));
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            backmenu = true;
            Destroy(GameObject.Find("ModeData"));
            if (SceneManager.GetActiveScene().name == "photon_in")
            {
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("MainMenu");
            }
                
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
